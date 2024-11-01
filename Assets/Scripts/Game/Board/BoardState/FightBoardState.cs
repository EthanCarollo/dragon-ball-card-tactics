using UnityEngine;

public class FightBoardState : BoardState
{
    public BoardObject[,] boardBeforeFight;

    public FightBoardState(FightBoard board) : base(board)
    {
        boardBeforeFight = BoardUtils.DuplicateBoardObjectGrid(GameManager.Instance.boardCharacterArray);
    }

    public override void Update()
    {
        UpdateBoardForFight();
    }

    public override bool IsFighting()
    {
        return true;
    }

    private void UpdateBoardForFight()
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
                        if (boardChar.isPlayerCharacter)
                        {
                            alivePlayerCount++;
                        }
                        else
                        {
                            aliveEnemyCount++;
                        }
                    }
                }

                character.UpdateUi();
                character.Update();
            }
        }

        // Check if no players or no enemies are alive, and end the fight if so
        if (alivePlayerCount == 0 || aliveEnemyCount == 0)
        {
            EndFight();
        }
    }


    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        // TODO : implement the logics for go to the next fight !
        Debug.Log("Ending fight");
        GameManager.Instance.boardCharacterArray = boardBeforeFight;
        board.CreateBoard();
        board.UpdateState(new DefaultBoardState(board));
    }
}