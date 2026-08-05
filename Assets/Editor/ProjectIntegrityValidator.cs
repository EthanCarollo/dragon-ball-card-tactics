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
  }
#endif
