
using UnityEngine;

public class KikohaFightBoardState : BoardState
{
    public KikohaFightBoardState(FightBoard board, BoardObject boardObject1, BoardObject boardObject2) : base(board) {
        if(boardObject1 is BoardCharacter boardPlayer){
            if(!boardPlayer.isPlayerCharacter){
                boardObject1.gameObject.transform.position = board.BoardArray[board.BoardArray.GetLength(0)-1, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
                boardObject2.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
            } else {
                boardObject1.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
                boardObject2.gameObject.transform.position = board.BoardArray[board.BoardArray.GetLength(0)-1, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
            }
            InstantiateKikoha(boardPlayer);
        }
        
        if(boardObject2 is BoardCharacter boardEnemy){
            InstantiateKikoha(boardEnemy);
        }
    }

    private void InstantiateKikoha(BoardCharacter boardChar){
        BoardAnimation attackAnimation = boardChar.character.GetCharacterData().specialAttackAnimation[0].animation;
        if(attackAnimation is ChargedKiAttackAnimation kiAttackAnimation){
            var boardAnimation = ScriptableObject.CreateInstance<BoardAnimation>();
            boardAnimation.frameSprites = boardChar.character.GetCharacterData().specialAttackAnimation[0].animation.frameSprites;
            boardChar.PlayAnimation(boardAnimation);
            boardChar.LaunchKikoha();
        }
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
                character.Update();
            }
        }
    }

    public override void LaunchKikohaFight(BoardObject boardObject1, BoardObject boardObject2)
    {

    }

    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        
    }
}
