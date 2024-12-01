using UnityEngine;

[CreateAssetMenu(fileName = "FightDatabase", menuName = "Fight/FightDatabase")]
public class FightDatabase : ScriptableObject
{
    private static FightDatabase _instance;
    
    public Fight[] fights;

    public Fight GetRandomFight()
    {
        return fights[Random.Range(0, fights.Length)];
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
                    Debug.LogError("ShadersDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
        
}