using UnityEngine;
using System.Linq;

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
        
}