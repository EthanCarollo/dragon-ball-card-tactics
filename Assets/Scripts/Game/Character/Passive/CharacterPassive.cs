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

        public virtual int AdditionalArmor(CharacterContainer character)
        {
                return 0;
        }

        public virtual void GetHit(int amount, BoardCharacter character)
        {
                
        }

        // Called when we additionnal hit
        public virtual void HitCharacter(BoardCharacter character, BoardCharacter target)
        {

        }

        // Called when we kill an enemy
        public virtual void KilledAnEnemy(BoardCharacter character, BoardCharacter target)
        {

        }
        
        public virtual void Setup(BoardCharacter character)
        {
                
        }
        
        public virtual void UpdatePassive(BoardCharacter character)
        {
                
        }
}