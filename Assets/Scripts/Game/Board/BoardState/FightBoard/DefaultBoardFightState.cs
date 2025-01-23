using UnityEngine;

public class DefaultBoardFightState : BoardFightState
{
    public bool isActive = true;
    
    public DefaultBoardFightState(FightBoardState boardFightState) : base(boardFightState)
    {
        
    }

    public override void Update()
    {
        int alivePlayerCount = 0;
        int aliveEnemyCount = 0;

        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;

                if (character is BoardCharacter boardChar)
                {
                    // Check if character is alive
                    if (!boardChar.character.IsDead())
                    {
                        if (boardChar.character.isPlayerCharacter)
                        {
                            alivePlayerCount++;
                        }
                        else
                        {
                            aliveEnemyCount++;
                        }
                    }
                    else
                    {
                        // Debug.Log("Character is dead : " + boardChar.character.GetName());
                    }
                }
                
                character.UpdateUi();
                if (isActive)
                {
                    character.Update();
                }
            }
        }

        // Check if no players or no enemies are alive, and end the fight if so
        if (alivePlayerCount == 0 || aliveEnemyCount == 0)
        {
            Debug.Log("There is no alive character actually in this board, player : " + alivePlayerCount + " enemy : " + aliveEnemyCount );
            boardFightState.EndFight(alivePlayerCount > 0);
        }
    }

    public override void LaunchCinematic()
    {
        Debug.Log("Call launch cinematic");
        isActive = false;
        boardFightState.UpdateState(new CinematicBoardFightState(boardFightState));
    }

    public override void EndCinematic()
    {
        
    }
}