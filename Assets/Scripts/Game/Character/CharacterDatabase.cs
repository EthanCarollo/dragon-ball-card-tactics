using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewCharacterDatabase", menuName = "Character/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    private static CharacterDatabase _instance;
    
    public CharacterData[] characterDatas;

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
                AssignUniqueIDs();
            }
            return _instance;
        }
    }

    private static void AssignUniqueIDs()
    {
        if (_instance.characterDatas != null)
        {
            for (int i = 0; i < _instance.characterDatas.Length; i++)
            {
                _instance.characterDatas[i].id = i;
            }
        }
    }
    
    public CharacterData GetCharacterById(int id)
    {
        if (characterDatas != null)
        {
            foreach (var character in characterDatas)
            {
                if (character.id == id)
                {
                    return character;
                }
            }
        }
        Debug.LogWarning($"Character with ID {id} not found.");
        return null; 
    }
}
