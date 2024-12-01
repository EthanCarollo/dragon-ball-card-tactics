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
        // TODO : implement the logics for go to the next fight !
        Debug.Log("Ending fight");
        board.StartCoroutine(EndFightCoroutine());
    }

    private bool userClicked = false;

    public IEnumerator EndFightCoroutine(bool win = true)
    {
        Debug.Log("Ending fight coroutine called");
        board.UpdateState(new DefaultBoardState(board));
        EndFightPanelUi.Instance.SetupEndFightPanel();
        Button endFightButton = EndFightPanelUi.Instance.EndFightButton;
        endFightButton.onClick.AddListener(OnUserClick);
        while (!userClicked)
        {
            yield return null; 
        }
        endFightButton.onClick.RemoveListener(OnUserClick);
        EndFightPanelUi.Instance.CloseEndFightPanel();
        GameManager.Instance.boardCharacterArray = boardBeforeFight;
        board.CreateBoard();
    }

    private void OnUserClick()
    {
        userClicked = true;
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