using UnityEngine;

public static class BoardUtils
{
    public static Vector2Int GetCharacterPosition(BoardObject[,] board, BoardObject character)
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == character)
                {
                    return new Vector2Int(x, y); 
                }
            }
        }
        return new Vector2Int(-1, -1); 
    }

    public static bool MoveCharacter(BoardObject[,] board, BoardObject character, Vector2Int targetPosition)
    {
        Vector2Int currentPosition = BoardUtils.GetCharacterPosition(board, character);
        if (targetPosition.x < 0 || targetPosition.x >= board.GetLength(0) || targetPosition.y < 0 || targetPosition.y >= board.GetLength(1))
        {
            Debug.LogWarning("Target position is out of bounds.");
            return false;
        }
        if (board[targetPosition.x, targetPosition.y] != null)
        {
            Debug.LogWarning("Target position is already occupied.");
            return false; 
        }
        board[targetPosition.x, targetPosition.y] = character;
        board[currentPosition.x, currentPosition.y] = null;
        return true; 
    }
    
    public static Vector2Int? GetFirstEmptyAround(BoardObject[,] boardCharacters, BoardObject fromCharacter, BoardObject toCharacter, int range)
    {
        Vector2Int fromPosition = GetCharacterPosition(boardCharacters, fromCharacter);
        Vector2Int toPosition = GetCharacterPosition(boardCharacters, toCharacter);
        var aStar = new AStarPathfinding(boardCharacters);

        if (fromPosition.x == -1 && fromPosition.y == -1)
        {
            Debug.LogWarning("From character not found on the board.");
            return null;
        }

        Vector2Int? closestEmptyPosition = null;
        float closestDistance = float.MaxValue;

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                // Skip positions that are outside the diamond shape
                if (Mathf.Abs(x) + Mathf.Abs(y) > range) continue;
                if (x == 0 && y == 0) continue; // Skip the toPosition itself

                Vector2Int neighborPosition = toPosition + new Vector2Int(x, y);

                if (neighborPosition.x >= 0 && neighborPosition.x < boardCharacters.GetLength(0) &&
                    neighborPosition.y >= 0 && neighborPosition.y < boardCharacters.GetLength(1))
                {
                    if (boardCharacters[neighborPosition.x, neighborPosition.y] == fromCharacter)
                    {
                        // Direct neighbor position found
                        return neighborPosition;
                    }

                    if (boardCharacters[neighborPosition.x, neighborPosition.y] == null)
                    {
                        var tempPath = aStar.FindPath(fromPosition, neighborPosition);
                        if (tempPath != null && tempPath.Count < closestDistance)
                        {
                            closestDistance = tempPath.Count;
                            closestEmptyPosition = neighborPosition;
                        }
                    }
                }
            }
        }

        return closestEmptyPosition;
    }

    public static bool AddCharacter(BoardObject[] board, BoardCharacter character)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == null)
            {
                board[i] = character;
                return true;
            }
        }
        return false;
    }
    
    public static Vector2 GetDirectionVector(Vector2 direction)
    {
        direction.Normalize();
        if (Vector2.Dot(direction, Vector2.right) > 0.7f)
        {
            return Vector2.right; 
        }
        else if (Vector2.Dot(direction, Vector2.left) > 0.7f)
        {
            return Vector2.left;  
        }
        else if (Vector2.Dot(direction, Vector2.up) > 0.7f)
        {
            return Vector2.up;    
        }
        else if (Vector2.Dot(direction, Vector2.down) > 0.7f)
        {
            return Vector2.down;   
        }
        else
        {
            return direction;     
        }
    }

    public static void InflictDamageInZone(Vector2Int[] tiles, int damage)
    {
        BoardObject[,] boardChar = GameManager.Instance.boardCharacterArray;

        foreach (Vector2Int tile in tiles)
        {
            if (tile.x >= 0 && tile.x < boardChar.GetLength(0) && tile.y >= 0 && tile.y < boardChar.GetLength(1))
            {
                BoardObject boardObject = boardChar[tile.x, tile.y];
                if (boardObject is BoardCharacter character)
                {
                    character.HitDamage(damage);
                }
            }
        }
    }

    public static BoardObject[,] DuplicateBoardObjectGrid(BoardObject[,] board)
    {
        BoardObject[,] duplicateBoard = new BoardObject[board.GetLength(0), board.GetLength(1)];
        
        for (int i = 0; i < duplicateBoard.GetLength(0); i++)
        {
            for (int j = 0; j < duplicateBoard.GetLength(1); j++)
            {
                // Use the Clone method to duplicate each BoardObject
                duplicateBoard[i, j] = board[i, j]?.Clone();
            }
        }
        return duplicateBoard;
    }
}
