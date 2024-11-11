
using UnityEngine;

public class KikohaFightBoardState : BoardState
{
    private BoardCharacter character1;
    private BoardCharacter character2;

    public KikohaFightBoardState(FightBoard board, BoardObject boardObject1, BoardObject boardObject2) : base(board) {
        if(boardObject1 is BoardCharacter boardPlayer){
            if(!boardPlayer.isPlayerCharacter){
                character1 = boardPlayer;
                boardObject1.gameObject.transform.position = board.BoardArray[board.BoardArray.GetLength(0)-1, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
                boardObject2.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
            } else {
                character1 = boardPlayer;
                boardObject1.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
                boardObject2.gameObject.transform.position = board.BoardArray[board.BoardArray.GetLength(0)-1, board.BoardArray.GetLength(1)/2].gameObject.transform.position;
            }
            InstantiateKikoha(boardPlayer);
        }
        
        if(boardObject2 is BoardCharacter boardEnemy){
            character2 = boardEnemy;
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

    private float updateInterval = 0.1f;
    private float timer = 0f;
    
    public override void Update()
    {
        character1.Update();
        character1.UpdateUi();
        character2.Update();
        character2.UpdateUi();

        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            int currentAdvancement1 = character1.GetKikohaAdvancement();
            int targetAdvancement1 = 100;
            int advancementStep1 = 1; // Increment by 1 each interval

            if (currentAdvancement1 < targetAdvancement1)
            {
                character1.UpdateKikohaAdvancement(Mathf.Min(currentAdvancement1 + advancementStep1, targetAdvancement1));
            }

            int currentAdvancement2 = character2.GetKikohaAdvancement();
            int targetAdvancement2 = 0;
            int advancementStep2 = 1; 

            if (currentAdvancement2 > targetAdvancement2)
            {
                character2.UpdateKikohaAdvancement(Mathf.Max(currentAdvancement2 - advancementStep2, targetAdvancement2));
            }
        }
    }

    public override void LaunchKikohaFight(BoardObject boardObject1, BoardObject boardObject2)
    {
        
    }

    public override void EndKikohaFight()
    {

    }

    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        
    }
}
