using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Fusion Transform Animation", menuName = "BoardAnimation/FusionTransformAnimation")]
public class FusionTransformAnimation : TransformAnimation {
    public CharacterData characterData1;
    public CharacterData characterData2;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        bool isFusionForPlayer = character.character.isPlayerCharacter;
        BoardCharacter holder1 = null;
        BoardCharacter holder2 = null;
        
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var boardObject = GameManager.Instance.boardCharacterArray[x, y];
                if (boardObject == null) continue;

                if (boardObject is BoardCharacter boardChar)
                {
                    if (!boardChar.character.IsDead() && boardChar.character.isPlayerCharacter == isFusionForPlayer )
                    {
                        if (boardChar.character.GetCharacterData() == characterData1)
                        {
                            holder1 = boardChar;
                        }
                        if (boardChar.character.GetCharacterData() == characterData2)
                        {
                            holder2 = boardChar;
                        }
                    }
                }
            }
        }

        if (holder1 != null && holder2 != null)
        {
            if (holder1 == character)
            {
                holder2.RemoveFromBoard();
            }
            else
            {
                holder1.RemoveFromBoard();
            }
            character.actualAnimation = this;
            yield return PlayAnimationCoroutineTransform(character);
            character.SetupCharacter(new CharacterContainer(newCharacterData.id, newCharacterData.maxHealth, 1, 0, character.character.isPlayerCharacter));
            EndAnimation(character);
        }
    }
    
    public override bool CanTransform(BoardCharacter character)
    {
        bool isFusionForPlayer = character.character.isPlayerCharacter;
        BoardCharacter holder1 = null;
        BoardCharacter holder2 = null;
        
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var boardObject = GameManager.Instance.boardCharacterArray[x, y];
                if (boardObject == null) continue;

                if (boardObject is BoardCharacter boardChar)
                {
                    if (!boardChar.character.IsDead() && boardChar.character.isPlayerCharacter == isFusionForPlayer )
                    {
                        if (boardChar.character.GetCharacterData() == characterData1)
                        {
                            holder1 = boardChar;
                        }
                        if (boardChar.character.GetCharacterData() == characterData2)
                        {
                            holder2 = boardChar;
                        }
                    }
                }
            }
        }
        
        if (holder1 != null && holder2 != null)
        {
            return true;
        }
        return false;
    }
}