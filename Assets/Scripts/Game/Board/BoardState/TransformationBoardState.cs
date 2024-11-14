using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TransformationBoardState : BoardState
{
    public TransformationBoardState(FightBoard board, BoardObject boardObject1, TransformAnimation transformation) : base(board)
    {
        ResetEveryCharacterState();
        if(CameraScript.Instance != null){
            CameraScript.Instance.SetupTransformationCamera(boardObject1);
        }

        if (boardObject1 is BoardCharacter boardCharacter)
        {
            boardCharacter.Transform(transformation);
        }
        
        board.StartCoroutine(EndAnimation(transformation));
    }

    public void ResetEveryCharacterState()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                if (character is BoardCharacter boardCharacter)
                {
                    boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
                }
            }
        }
    }

    private IEnumerator EndAnimation(TransformAnimation transformation)
    {
        foreach (var frame in transformation.frameSprites)
        {
            yield return new WaitForSeconds(frame.time);
        }
        yield return new WaitForSeconds(0.4f);
        board.UpdateState(new FightBoardState(board, false));
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

    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        
    }
        
}