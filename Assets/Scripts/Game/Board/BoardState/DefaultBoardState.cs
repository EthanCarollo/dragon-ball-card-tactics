
using System.Collections.Generic;
using UnityEngine;

public class DefaultBoardState : BoardState
{
    public bool isCinematic = false;
    
    public DefaultBoardState(FightBoard board) : base(board) { 
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupNormalCamera();
        }
    }

    public override void Start()
    {
        BoardGameUiManager.Instance.launchFightButton.SetActive(true);
        CardUi.Instance.ShowCardUi();
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
                if (character is BoardCharacter boardCharacter && isCinematic == false)
                {
                    boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.GetCharacterData().idleAnimation);
                }
            }
        }
    }

    public override void LaunchFight()
    {
        board.UpdateState(new FightBoardState(board));
    }

    public override void EndFight(bool win)
    {
        
    }


    public override void LaunchCinematic()
    {
        isCinematic = true;
        BoardGameUiManager.Instance.launchFightButton.SetActive(false);
        CardUi.Instance.HideCardUi();
    }
    public override void EndCinematic()
    {
        isCinematic = false;
        BoardGameUiManager.Instance.launchFightButton.SetActive(true);
        CardUi.Instance.ShowCardUi();
    }
}
