using UnityEngine;

public static class BoardUtils
{
    public static Vector2Int GetCharacterPosition(BoardCharacter[,] board, BoardCharacter character)
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

    public static bool MoveCharacter(BoardCharacter[,] board, BoardCharacter character, Vector2Int targetPosition)
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
    
    public static Vector2Int? GetFirstEmptyAround(BoardCharacter[,] boardCharacters, BoardCharacter fromCharacter, BoardCharacter toCharacter)
    {
        Vector2Int fromPosition = GetCharacterPosition(boardCharacters, fromCharacter);
        Vector2Int toPosition = GetCharacterPosition(boardCharacters, toCharacter);

        if (fromPosition.x == -1 && fromPosition.y == -1)
        {
            Debug.LogWarning("From character not found on the board.");
            return null;
        }

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(-1, 0), 
            new Vector2Int(1, 0), 
            new Vector2Int(0, -1), 
            new Vector2Int(0, 1),  
            new Vector2Int(-1, -1), 
            new Vector2Int(-1, 1),  
            new Vector2Int(1, -1), 
            new Vector2Int(1, 1)  
        };

        Vector2Int? closestEmptyPosition = null;
        float closestDistance = float.MaxValue;

        foreach (var direction in directions)
        {
            Vector2Int neighborPosition = toPosition + direction;

            if (neighborPosition.x >= 0 && neighborPosition.x < boardCharacters.GetLength(0) &&
                neighborPosition.y >= 0 && neighborPosition.y < boardCharacters.GetLength(1))
            {
                if (boardCharacters[neighborPosition.x, neighborPosition.y] == fromCharacter)
                {
                    return neighborPosition;
                }
                if (boardCharacters[neighborPosition.x, neighborPosition.y] == null)
                {
                    float distance = Vector2Int.Distance(fromPosition, neighborPosition);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEmptyPosition = neighborPosition;
                    }
                }
            }
        }

        return closestEmptyPosition;
    }
}
