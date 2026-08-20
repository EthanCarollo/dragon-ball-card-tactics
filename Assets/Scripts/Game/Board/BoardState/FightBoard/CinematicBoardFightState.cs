using UnityEngine;

public class CinematicBoardFightState : BoardFightState
{
    public CinematicBoardFightState(FightBoardState boardFightState) : base(boardFightState)
    {
        
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
