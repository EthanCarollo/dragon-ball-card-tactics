
using System;
using UnityEngine;

public class DefaultCharacterState : BoardCharacterState
{
    public DefaultCharacterState(BoardCharacter character) : base(character) { }
    
    public override void Update()
    {
        if (boardCharacter.IsDead())
        {
            return;
        }
        if (boardCharacter.nextPosition.x == -1 && boardCharacter.nextPosition.y == -1)
        {
            FindTarget();
        }
        else
        {
            try
            {
                MoveTowardsTarget();
            }
            catch (Exception e)
            {
                Debug.LogError("Try to move character to : " + boardCharacter.nextPosition.ToString() + " got error : " + e.ToString());
            }
        }
    }

    private void FindTarget()
    {
        BoardObject[,] boardCharacters = GameManager.Instance.boardCharacterArray;
        Vector2Int characterPosition = BoardUtils.GetCharacterPosition(boardCharacters, boardCharacter);
        var aStar = new AStarPathfinding(boardCharacters);
        var pathLengthToTarget = 9999;
        for (int x = 0; x < boardCharacters.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacters.GetLength(1); y++)
            {
                var boardObject = boardCharacters[x, y];
                if (boardObject == null) continue;
                if (boardObject is BoardCharacter character && character.isPlayerCharacter != boardCharacter.isPlayerCharacter)
                {
                    Vector2Int? emptyPosition = BoardUtils.GetFirstEmptyAround(boardCharacters, this.boardCharacter,  character);
                    if (emptyPosition.HasValue)
                    {
                        // If he is already in front of a character, just put it in AttackState so !
                        if (characterPosition == emptyPosition.Value)
                        {
                            boardCharacter.nextPosition = new Vector2Int(-1, -1);
                            boardCharacter.PlayAnimation(boardCharacter.character.idleSprites);
                            boardCharacter.UpdateState(new AttackingCharacterState(boardCharacter, character));
                            return;
                        }
                        try
                        {
                            var path = aStar.FindPath(characterPosition, emptyPosition.Value);
                            if (path == null && path.Count < 1)
                            {
                                continue;
                            }

                            if (path.Count > pathLengthToTarget) continue;
                        
                            pathLengthToTarget = path.Count;
                            boardCharacter.nextPosition = path[0];
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Got an error on finding path from " + boardCharacter.character.name + " to " + character.character.name + ": " + e);
                        }
                    }
                }
            }
        }
        BoardUtils.MoveCharacter(boardCharacters, boardCharacter, boardCharacter.nextPosition);
    }
    
    private void MoveTowardsTarget()
    {
        Vector2 targetPosition = boardCharacter.nextPosition;
        float step = boardCharacter.character.baseSpeed * Time.deltaTime;
        boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.runSprites);
        boardCharacter.gameObject.transform.position = Vector3.MoveTowards(boardCharacter.gameObject.transform.position, targetPosition, step);
        if (Vector3.Distance(boardCharacter.gameObject.transform.position, targetPosition) < 0.001f)
        {
            boardCharacter.gameObject.transform.position = targetPosition;
            boardCharacter.nextPosition = new Vector2Int(-1, -1);
        }
    }
    
    public override void Attack(int damage, GameObject particle)
    {
        Debug.LogWarning("Launch attack if default character");
    }
}
