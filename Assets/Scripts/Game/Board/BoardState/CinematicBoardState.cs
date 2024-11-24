using System.Collections;
using UnityEngine;
    
public class CinematicBoardState : BoardState
{
    public CinematicBoardState(FightBoard board, BoardCharacter boardChar) : base(board)
    {
        ResetEveryCharacterStateExcept(boardChar);
    }

    public void ResetEveryCharacterStateExcept(BoardCharacter boardChar)
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                if (character is BoardCharacter boardCharacter && boardChar != boardCharacter)
                {
                    Debug.Log("updated state : " + boardCharacter.character.GetCharacterData().characterName);
                    boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
                }
            }
        }
    }
    
    public override void Update()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                character.UpdateUi();
            }
        }
    }

    public override void LaunchCinematic(BoardCharacter boardChar)
    {
        
    }
    public override void EndCinematic()
    {
        this.board.UpdateState(new FightBoardState(board));
    }

    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        
    }
}