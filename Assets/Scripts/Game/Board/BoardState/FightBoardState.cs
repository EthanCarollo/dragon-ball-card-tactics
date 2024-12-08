using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FightBoardState : BoardState
{
    public BoardObject[,] boardBeforeFight;
    public BoardFightState boardState;

    public FightBoardState(FightBoard board, bool resetPassives = true) : base(board)
    {
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupFightCamera();
        }
        boardBeforeFight = BoardUtils.DuplicateBoardObjectGrid(GameManager.Instance.boardCharacterArray);
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

    public override void EndFight()
    {
        Debug.Log("Ending fight");
        board.UpdateState(new DefaultBoardState(board));
        WinFightUi.Instance.OpenWinFightUi(board);
        GameManager.Instance.boardCharacterArray = boardBeforeFight;
        GameManager.Instance.GoNextFight();
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