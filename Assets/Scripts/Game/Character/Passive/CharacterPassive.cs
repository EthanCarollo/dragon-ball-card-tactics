using System;
using UnityEngine;

[Serializable]
public class CharacterPassive : ScriptableObject
{
        public Sprite passiveImage;

        public string passiveName;
        public string passiveDescription;

        public virtual int AdditionalAttack(CharacterContainer character)
        {
                return 0;
        }

        public virtual void GetHit(int amount, BoardCharacter character)
        {
                
        }
        
        public virtual void Setup(BoardCharacter character)
        {
                
        }
        
        public virtual void UpdatePassive(BoardCharacter character)
        {
                
        }
}