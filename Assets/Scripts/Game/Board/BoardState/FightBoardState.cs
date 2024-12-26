using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightBoardState : BoardState
{
    public BoardObject[,] boardBeforeFight;
    public BoardObject[,] boardBeforeFightEmpty;
    public BoardFightState boardState;

    public FightBoardState(FightBoard board, bool resetPassives = true) : base(board)
    {
        BoardGameUiManager.Instance.launchFightButton.SetActive(false);
        CardUi.Instance.HideCardUi();
        
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupFightCamera();
        }

        boardBeforeFightEmpty = BoardUtils.DuplicateBoardObjectGrid(GameManager.Instance.boardCharacterArray, false);
        boardBeforeFight = BoardUtils.DuplicateBoardObjectGrid(GameManager.Instance.boardCharacterArray, true);
        boardState = new DefaultBoardFightState(this);

        if (resetPassives)
        {
            ResetAllPassives();
        }
    }
    
    public override void Start()
    {
        SetupSynergyStartAction();
    }

    public void SetupSynergyStartAction()
    {
        List<Synergy> ingameSynergy = GameManager.Instance.GetActiveSynergy();
        foreach (var synergy in ingameSynergy)
        {
            var tierBonuses = synergy.GetActiveTierBonuses();
            foreach (var tierBonus in tierBonuses)
            {
                foreach (var bonus in tierBonus.Bonuses)
                {
                    if(bonus.OnStartSetupAction(true) == true) return;
                }
            }
        }
    }

    public void UpdateState(BoardFightState boardState)
    {
        this.boardState = boardState;
    }

    public override void Update()
    {
        boardState.Update();
    }

    public override bool IsFighting()
    {
        return true;
    }


    public override void LaunchFight()
    {
        
    }

    public override void EndFight(bool win)
    {
        Debug.Log("Ending fight");
        board.UpdateState(new DefaultBoardState(board));
        GameManager.Instance.Player.Mana.AddMana(1);
        if(win == false){
            GameManager.Instance.boardCharacterArray = boardBeforeFight;
            GameManager.Instance.Player.Life.LooseLife(1);
            GameManager.Instance.GoNextFight();
        } else {
            GameManager.Instance.boardCharacterArray = boardBeforeFightEmpty;
            GameManager.Instance.Player.Level.AddExperience(3);
            if(GameManager.Instance.ActualFight.difficulty != FightDifficulty.Easy){
                WinFightUi.Instance.OpenWinFightUi(board);
            }else{
                GameManager.Instance.GoNextFight();
            }
        }
        BoardGameUiManager.Instance.characterBoardUi.HideCharacterBoard();
        BoardGameUiManager.Instance.RefreshUI();
        board.CreateBoard(GameManager.Instance.boardCharacterArray);
    }



    public override void EndCinematic()
    {
        boardState.EndCinematic();
        SetupSynergyStartAction();
    }

    public override void LaunchCinematic()
    {
        boardState.LaunchCinematic();
    }
}