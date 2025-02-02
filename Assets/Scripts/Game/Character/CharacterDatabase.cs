using UnityEngine;
using UnityEditor;
using UnityEngine.TextCore.Text;
using System.Linq;

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
                this.characterDatas[i].id = i;
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

#if UNITY_EDITOR
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

        foreach(var character in newCharacterDatas){
            if(characterDatas.Contains(character)) continue;
            if (character.sameCharacters == null){
                Debug.LogWarning("This character has same characters to null wtf : " + character.characterName);
                continue;
            }
            if(character.sameCharacters.Contains(null)){
                Debug.LogWarning("This character contains bad same character : " + character.characterName);
            }
            Debug.LogWarning("Add new character : " + character.characterName);
            var characterDataList = characterDatas.ToList();
            characterDataList.Add(character);
            characterDatas = characterDataList.ToArray();
        }

        this.AssignUniqueIDs();
        EditorUtility.SetDirty(this);
        Debug.Log("Character list refreshed!");
    }
#endif
}
