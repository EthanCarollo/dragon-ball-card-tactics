using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FightBoardState : BoardState
{
    public BoardObject[,] boardBeforeFight;
    public BoardFightState boardState;

    public FightBoardState(FightBoard board, bool resetPassives = true) : base(board)
    {
        BoardGameUiManager.Instance.launchFightButton.SetActive(false);
        CardUi.Instance.HideCardUi();
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupFightCamera();
        }
        boardBeforeFight = BoardUtils.DuplicateBoardObjectGrid(GameManager.Instance.boardCharacterArray, false);
        boardState = new DefaultBoardFightState(this);
        if (resetPassives)
        {
            ResetAllPassives();
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
        GameManager.Instance.boardCharacterArray = boardBeforeFight;
        board.UpdateState(new DefaultBoardState(board));
        GameManager.Instance.Player.Mana.AddMana(1);
        if(win == false){
            GameManager.Instance.Player.Life.LooseLife(1);
        } else {
            GameManager.Instance.Player.Level.AddExperience(3);
            WinFightUi.Instance.OpenWinFightUi(board);
            GameManager.Instance.GoNextFight();
        }
        BoardGameUiManager.Instance.RefreshUI();
        board.CreateBoard();
    }



    public override void EndCinematic()
    {
        boardState.EndCinematic();
    }

    public override void LaunchCinematic(BoardCharacter boardChar)
    {
        boardState.LaunchCinematic();
    }
}