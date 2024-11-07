using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog System/Dialog")]
[Serializable]
public class Dialog : ScriptableObject
{
    public CharacterData leftCharacter;
    public CharacterData rightCharacter;
    [TextArea(3, 10)]
    public string content;

    public virtual void GoNextDialog()
    {
        
    }
}