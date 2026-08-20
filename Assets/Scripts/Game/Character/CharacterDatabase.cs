using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
                    return null;
                }

                _instance.AssignUniqueIDs();
            }
            return _instance;
        }
    }

    public void AssignUniqueIDs()
    {
        if (this.characterDatas != null)
        {
            for (int i = 0; i < this.characterDatas.Length; i++)
            {
                if (this.characterDatas[i] != null)
                {
                    this.characterDatas[i].id = i;
                }
            }
        }
    }
    
    public CharacterData GetCharacterById(int id)
    {
        if (characterDatas != null)
        {
            foreach (var character in characterDatas)
            {
                if (character != null && character.id == id)
                {
                    return character;
                }
            }
        }
        Debug.LogWarning($"Character with ID {id} not found.");
        return null; 
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Databases/Normalize Character IDs")]
    public static void NormalizeCharacterIds()
    {
        var database = Resources.Load<CharacterDatabase>("CharacterDatabase");
        if (database == null || database.characterDatas == null)
        {
            Debug.LogError("CharacterDatabase instance not found or empty.");
            return;
        }

        for (int i = 0; i < database.characterDatas.Length; i++)
        {
            var character = database.characterDatas[i];
            if (character == null || character.id == i)
            {
                continue;
            }

            character.id = i;
            EditorUtility.SetDirty(character);
        }

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        Debug.Log($"Normalized IDs for {database.characterDatas.Length} characters.");
    }

    [ContextMenu("Refresh Character List")]
    public void RefreshCharacterList()
    {
        string[] guids = AssetDatabase.FindAssets("t:CharacterData");
        var newCharacterDatas = new CharacterData[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            newCharacterDatas[i] = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
        }

        var currentCharacters = characterDatas ?? new CharacterData[0];
        foreach(var character in newCharacterDatas){
            if(character == null || currentCharacters.Contains(character)) continue;
            if (character.sameCharacters == null){
                Debug.LogWarning("Character has no same-character family configured: " + character.characterName);
            }
            if(character.sameCharacters.Contains(null)){
                Debug.LogWarning("This character contains bad same character : " + character.characterName);
            }
            Debug.LogWarning("Add new character : " + character.characterName);
            var characterDataList = currentCharacters.ToList();
            characterDataList.Add(character);
            characterDatas = characterDataList.ToArray();
            currentCharacters = characterDatas;
        }

        this.AssignUniqueIDs();
        EditorUtility.SetDirty(this);
        Debug.Log("Character list refreshed!");
    }
#endif
}
