using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class ChargedKiAttackAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public Sprite kikoha;
    public Particle particleAttack;
    public AttackType attackType;
    public Vector2 startMargin;
    public Vector2Int kikohaSize;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            Debug.Log(character.direction);
            if(index == attackFrameIndex){
                LaunchAttack(character);
            }
            index++;
        }
        character.isAnimating = false;
    }

    private void LaunchAttack(BoardCharacter character)
    {
        GameObject newGameObject = new GameObject("SuperKikoha");
        newGameObject.transform.localScale = new Vector3(0, 1, 0);
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = kikoha;
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.sortingOrder = 5;
        newGameObject.transform.position = character.gameObject.transform.position + new Vector3(startMargin.x, startMargin.y);
        Vector3 direction = character.direction.normalized; // Assurez-vous que `character.direction` est un vecteur normalisé
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        try
        {
            newGameObject.LeanScale(new Vector3(1, 1, 0), 0.3f)
                .setOnComplete((() =>
                {
                    LeanTween.value(newGameObject, f => spriteRenderer.color = new Color(1f, 1f, 1f, f), 1f, 0f, 0.3f);
                    MonoBehaviour.Destroy(newGameObject, 0.3f);
                }));
        }
        catch (Exception error)
        {
            Debug.LogWarning("Error on moving game object of kikoha projectile, so delete it.");
            MonoBehaviour.Destroy(newGameObject);
        }
    }

    
}