using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    private static CharacterDatabase _instance;
    
    public CharactersContainer characterDatas;

    public static CharacterDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CharacterDatabase>("CharacterDatabase");
                if (_instance == null)
                {
                    Debug.LogError("CharacterDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
