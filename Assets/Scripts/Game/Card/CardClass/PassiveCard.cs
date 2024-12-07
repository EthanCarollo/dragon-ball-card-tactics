using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCard", menuName = "Card/PassiveCard")]
public class PassiveCard : UsableCharacterActionCard
{
    public CharacterPassive passive;
    public override void UseCard()
    {
        if (GetCharacterOnMouse() != null)
        {
            var index = Array.IndexOf(GetCharacterOnMouse().character.GetCharacterData().characterPassive, passive);
            if (index != -1)
            {
                GetCharacterOnMouse().character.unlockedPassives.Add(index);
                Debug.Log(GetCharacterOnMouse().character.unlockedPassives.ToString());
            }
            GameManager.Instance.RemoveCard(this);
        }
    }
}