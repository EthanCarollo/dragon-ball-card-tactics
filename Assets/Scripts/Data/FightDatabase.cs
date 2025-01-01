using UnityEngine;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "FightDatabase", menuName = "Fight/FightDatabase")]
public class FightDatabase : ScriptableObject
{
    private static FightDatabase _instance;
    
    public Fight[] fights;

    public Fight GetRandomFight(FightDifficulty difficulty = FightDifficulty.Easy)
    {
        var filteredFights = fights.Where(card => card.difficulty == difficulty).ToArray();

        if (filteredFights.Length == 0)
        {
            Debug.LogWarning($"No fights found with difficulty: {difficulty}");
            return null;
        }

        return filteredFights[Random.Range(0, filteredFights.Length)];
    }

    public static FightDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<FightDatabase>("FightDatabase");
                if (_instance == null)
                {
                    Debug.LogError("FightDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Fight List")]
    public void RefreshFights()
    {
        string[] guids = AssetDatabase.FindAssets("t:Fight");
        fights = new Fight[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            fights[i] = AssetDatabase.LoadAssetAtPath<Fight>(path);
        }

        EditorUtility.SetDirty(this); // Mark the database as dirty to save changes
        Debug.Log("Fight list refreshed!");
    }
    
    [ContextMenu("Log Characters Not in Fights")]
    public void LogCharactersNotInFights()
    {
        if (CharacterDatabase.Instance == null || CharacterDatabase.Instance.characterDatas == null)
        {
            Debug.LogError("CharacterDatabase is not initialized or empty.");
            return;
        }

        if (fights == null || fights.Length == 0)
        {
            Debug.LogWarning("No fights are present in the FightDatabase.");
            return;
        }

        // Collect all characters present in fights
        var charactersInFights = fights
            .SelectMany(fight => fight.opponents)
            .Select(opponent => opponent.characterData)
            .ToHashSet();

        // Compare with all characters in the CharacterDatabase
        var charactersNotInFights = CharacterDatabase.Instance.characterDatas
            .Where(character => !charactersInFights.Contains(character))
            .ToList();

        // Log each unused character
        foreach (var character in charactersNotInFights)
        {
            Debug.Log($"Character '<color=cyan>{character.name}</color>' (ID: {character.id}) is not used in any fight.");
        }

        // Log the summary of unused characters
        Debug.Log($"Summary: <color=yellow>{charactersNotInFights.Count}</color> characters are not used in fights out of a total of <color=yellow>{CharacterDatabase.Instance.characterDatas.Length}</color> characters.");

        // Calculate and log the percentage of fights by difficulty
        var totalFights = fights.Length;
        var difficultyGroups = fights.GroupBy(fight => fight.difficulty);

        foreach (var group in difficultyGroups)
        {
            var difficultyPercentage = (group.Count() / (float)totalFights) * 100;
            Debug.Log($"Difficulty '<color=green>{group.Key}</color>': <color=yellow>{group.Count()}</color> fights ({difficultyPercentage:F2}%).");
        }
    }

#endif
        
}