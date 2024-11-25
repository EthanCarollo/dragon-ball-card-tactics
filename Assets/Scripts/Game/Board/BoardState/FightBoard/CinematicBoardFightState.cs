using UnityEngine;

public class CinematicBoardFightState : BoardFightState
{
    public CinematicBoardFightState(FightBoardState boardFightState) : base(boardFightState)
    {
        
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

    public override void LaunchCinematic()
    {
        
    }

    public override void EndCinematic()
    {
        boardFightState.UpdateState(new DefaultBoardFightState(boardFightState));
    }
}