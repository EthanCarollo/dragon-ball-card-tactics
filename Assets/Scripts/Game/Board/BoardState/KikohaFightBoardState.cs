
using UnityEngine;

public class KikohaFightBoardState : BoardState
{
    private BoardCharacter character1;
    private BoardCharacter character2;

    public KikohaFightBoardState(FightBoard board, BoardObject boardObject1, BoardObject boardObject2) : base(board)
    {
        if (boardObject1 is BoardCharacter boardPlayer)
        {
            boardPlayer.PlayAnimation(SpriteDatabase.Instance.disappearAnimation);
        }
        
        if (boardObject2 is BoardCharacter boardEnemy)
        {
            boardEnemy.PlayAnimation(SpriteDatabase.Instance.disappearAnimation);
        }

        LeanTween.delayedCall(0.4f, o =>
        {
            if (boardObject1 is BoardCharacter boardPlayer)
            {
                if (!boardPlayer.isPlayerCharacter)
                {
                    character1 = boardPlayer;
                    boardObject1.gameObject.transform.position = board
                        .BoardArray[board.BoardArray.GetLength(0) - 1, board.BoardArray.GetLength(1) / 2].gameObject
                        .transform.position;
                    boardObject2.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1) / 2]
                        .gameObject.transform.position;
                }
                else
                {
                    character1 = boardPlayer;
                    boardObject1.gameObject.transform.position = board.BoardArray[0, board.BoardArray.GetLength(1) / 2]
                        .gameObject.transform.position;
                    boardObject2.gameObject.transform.position = board
                        .BoardArray[board.BoardArray.GetLength(0) - 1, board.BoardArray.GetLength(1) / 2].gameObject
                        .transform.position;
                }

                InstantiateKikoha(boardPlayer);
            }

            if (boardObject2 is BoardCharacter boardEnemy)
            {
                character2 = boardEnemy;
                InstantiateKikoha(boardEnemy);
            }
        });
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
    private bool isKikohaFightEnded = false;
    
    public override void Update()
    {
        if (character1 == null || character2 == null) return;
        character1.Update();
        character1.UpdateUi();
        character2.Update();
        character2.UpdateUi();

        if (isKikohaFightEnded)
        {
            return;
        }
        
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
        
        if(character1.GetKikohaAdvancement() >= 95){
            winner = character1;
            looser = character2;
            EndKikohaFight();
            return;
        }
        if(character2.GetKikohaAdvancement() >= 95)
        {
            winner = character2;
            looser = character1;
            EndKikohaFight();
            return;
        }
    }

    public override void LaunchKikohaFight(BoardObject boardObject1, BoardObject boardObject2)
    {
        
    }

    private BoardCharacter winner = null;
    private BoardCharacter looser = null;

    public override void EndKikohaFight()
    {
        isKikohaFightEnded = true;
        LeanTween.value(character2.gameObject, f =>
        {
            character2.UpdateKikohaAdvancement(Mathf.FloorToInt(f));
        }, character2.GetKikohaAdvancement(), 0, 0.2f);
        LeanTween.value(character1.gameObject, f =>
        {
            character1.UpdateKikohaAdvancement(Mathf.FloorToInt(f));
        }, character1.GetKikohaAdvancement(), 300, 1f).setOnComplete(() =>
        {
            looser.HitDamage(winner.character.GetAttackDamage() * 10);
            for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
            {
                for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
                {
                    var character = GameManager.Instance.boardCharacterArray[x, y];
                    if (character == null) continue;
                    if (character is BoardCharacter boardCharacter) {
                        boardCharacter.EndKikoha();
                    }
                }
            }

            LeanTween.delayedCall(0.4f, () =>
            {
                board.CreateBoard();
                board.UpdateState(new FightBoardState(board));
            });
        });
    }

    public override void LaunchFight()
    {
        
    }

    public override void EndFight()
    {
        
    }
}
