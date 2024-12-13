
public class DefaultBoardState : BoardState
{
    public DefaultBoardState(FightBoard board) : base(board) { 
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupNormalCamera();
        }
    }
    
    public override void Update()
    {
        BoardGameUiManager.Instance.launchFightButton.SetActive(true);
        CardUi.Instance.ShowCardUi();
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

    public override void LaunchFight()
    {
        board.UpdateState(new FightBoardState(board));
    }

    public override void EndFight(bool win)
    {
        
    }

    public override void EndCinematic()
    {
        
    }

    public override void LaunchCinematic(BoardCharacter boardChar)
    {
        
    }
}
