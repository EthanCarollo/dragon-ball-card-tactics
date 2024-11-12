using System;

[Serializable]
public class CharacterPassive
{
        
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