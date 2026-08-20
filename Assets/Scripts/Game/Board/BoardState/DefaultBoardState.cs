
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
        BoardGameUiManager.Instance?.launchFightButton?.SetActive(true);
        CardUi.Instance?.ShowCardUi();
    }

    public override void Update()
    {
        var boardCharacters = GameManager.Instance?.boardCharacterArray;
        if (boardCharacters == null)
        {
            return;
        }

        for (int x = 0; x < boardCharacters.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacters.GetLength(1); y++)
            {
                var character = boardCharacters[x, y];
                if (character == null) continue;
                character.UpdateUi();
                if (character is BoardCharacter boardCharacter && boardCharacter.character != null && isCinematic == false)
                {
                    // On default board state every character are full life
                    boardCharacter.character.actualHealth = boardCharacter.character.GetCharacterMaxHealth();
                    boardCharacter.SetCharacterSlider();
                    
                    var characterData = boardCharacter.character.GetCharacterData();
                    boardCharacter.PlayAnimationIfNotRunning(characterData?.idleAnimation);
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
        BoardGameUiManager.Instance?.launchFightButton?.SetActive(false);
        CardUi.Instance?.HideCardUi();
    }
    public override void EndCinematic()
    {
        isCinematic = false;
        BoardGameUiManager.Instance?.launchFightButton?.SetActive(true);
        CardUi.Instance?.ShowCardUi();
    }
}
