using System;
using UnityEngine;

[Serializable]
public class CharacterPassive
{
        public Sprite passiveImage;

        public virtual string GetName()
        {
                return "Not a passive";
        }
        
        public virtual string GetDescription()
        {
                return "Not a passive";
        }

        public virtual int AdditionalAttack(CharacterContainer character)
        {
                return 0;
        }
        
        public virtual void Setup(BoardCharacter character)
        {
                
        }
        
        public virtual void Update(BoardCharacter character)
        {
                
        }
}