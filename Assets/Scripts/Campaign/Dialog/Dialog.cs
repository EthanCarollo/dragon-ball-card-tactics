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
    public bool leftTalk = false;
    
    [SerializeReference, SubclassSelector]
    public DialogStrategy dialogStrategy = new DialogStrategy();

    public virtual void GoNextDialog()
    {
        dialogStrategy.ExecuteStrategy(this);
    }
}

[Serializable]
public class DialogStrategy
{
    public virtual void ExecuteStrategy(Dialog dialog)
    {
        
    }
}

[Serializable]
public class SwitchPanelStrategy : DialogStrategy
{
    public int panel;
    
    public override void ExecuteStrategy(Dialog dialog)
    {
        PanelSwitcher.Instance.SwitchToPanel(panel);
    }
}