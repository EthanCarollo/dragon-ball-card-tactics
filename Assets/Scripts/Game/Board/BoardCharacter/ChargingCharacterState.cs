using UnityEngine;

public class ChargingCharacterState : BoardCharacterState
{
    public ChargingCharacterState(BoardCharacter character) : base(character)
    {
        character.character.actualKi = 0;
        LaunchAnimation();
    }

    public void LaunchAnimation()
    {
        var anim = this.boardCharacter.character.GetCharacterData().specialAttackAnimation[0].animation;
        if(anim is ChargedKiAttackAnimation chargedAnimation){
            GameObject newGameObject = new GameObject("Projectile");
            newGameObject.transform.localScale = new Vector3(1, 1, 0);
            SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = chargedAnimation.kikoha;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.sortingOrder = 5;
            spriteRenderer.size = new Vector2(0, spriteRenderer.size.y);

            newGameObject.transform.position = boardCharacter.gameObject.transform.position + new Vector3((chargedAnimation.startMargin.x * boardCharacter.direction.x), chargedAnimation.startMargin.y);

            Vector3 direction = boardCharacter.direction.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            var delay = 0f;
            var index = 0;
            var animationEnded = false;

            while(animationEnded == false){
                if(index == chargedAnimation.attackFrameIndex){
                    break;
                }
                delay+=chargedAnimation.frameSprites[index].time;
                index++;
            }

            kikohaGameobject = newGameObject;
            LeanTween.value(boardCharacter.gameObject, (f) => { spriteRenderer.size = new Vector2(f, spriteRenderer.size.y); }, 0f, 4.5f, 0.3f)
                .setDelay(delay)
                .setOnComplete(() => {
                    animateKikoha = true;
                });
        }
    }
    
    const float maxSize = 9f;
    private int percentage = 50;
    private GameObject kikohaGameobject = null;
    private bool animateKikoha = false;

    public override void Update()
    {
        if(animateKikoha == true){
            SpriteRenderer spriteRenderer = kikohaGameobject.GetComponent<SpriteRenderer>();
            float shakeAmount = Mathf.Sin(Time.time * 50) * 0.003f; 
            float sizeKikoha = maxSize * percentage / 100;
            spriteRenderer.size = new Vector2(sizeKikoha + shakeAmount, spriteRenderer.size.y);
            if(percentage >= 95){
                if(boardCharacter.board is FightBoard fightBoard){
                    fightBoard.EndKikohaFight();
                }
            }
        }
    }

    public override void UpdateKikohaAdvancement(int percentage){
        this.percentage = percentage;
    }
    public override int GetKikohaAdvancement(){
        return percentage;
    }
    public override void EndKikoha(){
        MonoBehaviour.Destroy(kikohaGameobject);
        boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
    }
    
    public override void Attack(int damage, Particle particle)
    {
        
    }
}