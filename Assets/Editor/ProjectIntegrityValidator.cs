#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class ProjectIntegrityValidator
{
    private static readonly Regex GuidReferencePattern = new Regex(
        @"guid: ([0-9a-f]{32})",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly string[] SerializedAssetExtensions =
    {
        ".asset", ".controller", ".mat", ".prefab", ".playable", ".scenetemplate", ".unity"
    };

    [MenuItem("Tools/Project/Validate Serialized References")]
      public static void ValidateSerializedReferences()
    {
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

        var referencesByGuid = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var filePath in Directory.EnumerateFiles(Application.dataPath, "*", SearchOption.AllDirectories))
        {
            if (!SerializedAssetExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            foreach (Match match in GuidReferencePattern.Matches(File.ReadAllText(filePath)))
            {
                var guid = match.Groups[1].Value;
                if (guid.StartsWith("0000000000000000", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!referencesByGuid.TryGetValue(guid, out var files))
                {
                    files = new HashSet<string>();
                    referencesByGuid.Add(guid, files);
                }

                files.Add(filePath.Replace('\\', '/'));
            }
        }

        var missingReferences = referencesByGuid
            .Where(reference => string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(reference.Key)))
            .OrderBy(reference => reference.Key)
            .ToList();

        foreach (var missingReference in missingReferences)
        {
            Debug.LogError(
                $"Missing serialized GUID {missingReference.Key} referenced by: " +
                string.Join(", ", missingReference.Value.Select(Path.GetFileName).OrderBy(name => name)));
        }

          Debug.Log(
              $"Serialized reference validation complete. " +
              $"Unique GUIDs: {referencesByGuid.Count}, missing GUIDs: {missingReferences.Count}.");

          if (missingReferences.Count > 0)
          {
              throw new InvalidOperationException(
                  $"Serialized reference validation failed with {missingReferences.Count} missing GUID(s).");
          }
      }

      [MenuItem("Tools/Project/Audit Asset Usage")]
      public static void AuditAssetUsage()
      {
          AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

          var referencedGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
          var serializedFiles = Directory.EnumerateFiles(Application.dataPath, "*", SearchOption.AllDirectories)
              .Where(filePath => SerializedAssetExtensions.Contains(
                  Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase))
              .ToList();

          foreach (var filePath in serializedFiles)
          {
              foreach (Match match in GuidReferencePattern.Matches(File.ReadAllText(filePath)))
              {
                  var guid = match.Groups[1].Value;
                  if (!guid.StartsWith("0000000000000000", StringComparison.OrdinalIgnoreCase))
                  {
                      referencedGuids.Add(guid);
                  }
              }
          }

          var unusedAssets = serializedFiles
              .Select(filePath => new
              {
                  FilePath = filePath,
                  AssetPath = ToAssetPath(filePath)
              })
              .Where(asset => !IsUsageAuditRoot(asset.AssetPath))
              .Select(asset => new
              {
                  asset.AssetPath,
                  Guid = AssetDatabase.AssetPathToGUID(asset.AssetPath)
              })
              .Where(asset => !string.IsNullOrEmpty(asset.Guid) && !referencedGuids.Contains(asset.Guid))
              .Select(asset => asset.AssetPath)
              .OrderBy(assetPath => assetPath, StringComparer.OrdinalIgnoreCase)
              .ToList();

          foreach (var unusedAsset in unusedAssets)
          {
              Debug.LogWarning($"Serialized asset is not referenced by another serialized asset: {unusedAsset}");
          }

          Debug.Log(
              $"Asset usage audit complete. Serialized files: {serializedFiles.Count}, " +
              $"potentially unused assets: {unusedAssets.Count}. No assets were deleted.");
      }

      private static string ToAssetPath(string absolutePath)
      {
          var relativePath = absolutePath.Substring(Application.dataPath.Length + 1)
              .Replace('\\', '/');
          return "Assets/" + relativePath;
      }

      private static bool IsUsageAuditRoot(string assetPath)
      {
          return assetPath.StartsWith("Assets/Resources/", StringComparison.OrdinalIgnoreCase) ||
                 assetPath.StartsWith("Assets/Editor/", StringComparison.OrdinalIgnoreCase) ||
                 assetPath.StartsWith("Assets/ThirdParty/", StringComparison.OrdinalIgnoreCase) ||
                 assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase) ||
                 assetPath.EndsWith(".unity", StringComparison.OrdinalIgnoreCase);
      }

      [MenuItem("Tools/Project/Validate Runtime Data")]
      public static void ValidateRuntimeData()
      {
          AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

          var errors = new List<string>();
          ValidateCards(Resources.Load<CardDatabase>("CardDatabase"), errors);
          ValidateCharacters(Resources.Load<CharacterDatabase>("CharacterDatabase"), errors);
          ValidateFights(Resources.Load<FightDatabase>("FightDatabase"), errors);

          foreach (var error in errors)
          {
              Debug.LogError(error);
          }

          Debug.Log($"Runtime data validation complete. Errors: {errors.Count}.");
          if (errors.Count > 0)
          {
              throw new InvalidOperationException(
                  $"Runtime data validation failed with {errors.Count} error(s).");
          }
      }

      private static void ValidateCards(CardDatabase database, ICollection<string> errors)
      {
          if (database == null)
          {
              errors.Add("CardDatabase is missing from Resources.");
              return;
          }

          ValidateUniqueAssets(database.cards, "CardDatabase.cards", errors);
          ValidateUniqueAssets(database.selectableCards, "CardDatabase.selectableCards", errors);
          ValidateUniqueAssets(database.playerCards, "CardDatabase.playerCards", errors);

          foreach (var card in database.cards ?? Array.Empty<Card>())
          {
              if (card == null)
              {
                  continue;
              }

              if (card.manaCost < 0)
              {
                  errors.Add($"Card '{card.name}' has a negative mana cost.");
              }

              if (card is CharacterCard characterCard && characterCard.character == null)
              {
                  errors.Add($"Character card '{card.name}' has no character assigned.");
              }

              if (card is TransformationCard transformationCard)
              {
                  if (transformationCard.transformations == null || transformationCard.transformations.Length == 0)
                  {
                      errors.Add($"Transformation card '{card.name}' has no transformation configured.");
                      continue;
                  }

                  foreach (var transformation in transformationCard.transformations)
                  {
                      if (transformation == null || transformation.character == null)
                      {
                          errors.Add($"Transformation card '{card.name}' has a missing source character.");
                          continue;
                      }

                      if (transformation.transformation == null ||
                          transformation.transformation.newCharacterData == null)
                      {
                          errors.Add(
                              $"Transformation card '{card.name}' for '{transformation.character.characterName}' " +
                              "has no destination character.");
                      }
                  }
              }
          }
      }

      private static void ValidateCharacters(CharacterDatabase database, ICollection<string> errors)
      {
          if (database == null)
          {
              errors.Add("CharacterDatabase is missing from Resources.");
              return;
          }

          ValidateUniqueAssets(database.characterDatas, "CharacterDatabase.characterDatas", errors);
          var ids = new HashSet<int>();
          foreach (var character in database.characterDatas ?? Array.Empty<CharacterData>())
          {
              if (character == null)
              {
                  continue;
              }

              if (!ids.Add(character.id))
              {
                  errors.Add($"Character '{character.characterName}' reuses ID {character.id}.");
              }

              if (string.IsNullOrWhiteSpace(character.characterName))
              {
                  errors.Add("A CharacterData asset has no character name.");
              }

              if (character.characterPrefab == null)
              {
                  errors.Add($"Character '{character.characterName}' has no character prefab.");
              }
          }
      }

      private static void ValidateFights(FightDatabase database, ICollection<string> errors)
      {
          if (database == null)
          {
              errors.Add("FightDatabase is missing from Resources.");
              return;
          }

          ValidateUniqueAssets(database.fights, "FightDatabase.fights", errors);
          foreach (var fight in database.fights ?? Array.Empty<Fight>())
          {
              if (fight == null)
              {
                  continue;
              }

              if (fight.opponents == null || fight.opponents.Length == 0)
              {
                  errors.Add($"Fight '{fight.name}' has no opponents.");
                  continue;
              }

              var positions = new HashSet<Vector2Int>();
              foreach (var opponent in fight.opponents)
              {
                  if (opponent == null || opponent.characterData == null)
                  {
                      errors.Add($"Fight '{fight.name}' has an opponent without character data.");
                      continue;
                  }

                  if (!positions.Add(opponent.position))
                  {
                      errors.Add($"Fight '{fight.name}' has multiple opponents at {opponent.position}.");
                  }

                  if (opponent.position.x < 0 || opponent.position.x >= GameManager.BoardWidth ||
                      opponent.position.y < 0 || opponent.position.y >= GameManager.BoardHeight)
                  {
                      errors.Add($"Fight '{fight.name}' places '{opponent.characterData.characterName}' outside the board.");
                  }
              }
          }
      }

      private static void ValidateUniqueAssets<T>(IEnumerable<T> assets, string fieldName, ICollection<string> errors)
          where T : UnityEngine.Object
      {
          var seen = new HashSet<T>();
          foreach (var asset in assets ?? Array.Empty<T>())
          {
              if (asset == null)
              {
                  errors.Add($"{fieldName} contains a null entry.");
                  continue;
              }

              if (!seen.Add(asset))
              {
                  errors.Add($"{fieldName} contains duplicate asset '{asset.name}'.");
              }
          }
      }
  }
#endif
