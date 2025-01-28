using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Charged Ki Animation", menuName = "BoardAnimation/ChargedKiAnimation")]
public class ChargedKiAttackAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public Sprite kikoha;
    public Particle particleAttack;
    public AttackType attackType;
    public int attackMultiplicator = 3;
    public Vector2 startMargin;
    public Vector2Int kikohaSize;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        CameraScript.Instance.SetupCameraOnTarget(3.5f, character.gameObject.transform);
        var target = character.GetCharacterTarget().gameObject.transform;
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.LaunchCinematic();
        }
        character.actualAnimation = this;
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
        yield return new WaitForSeconds(0.25f); 
        
        EndAnimation(character);
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        
        if (character.board is FightBoard fightBoard2)
        {
            fightBoard2.EndCinematic();
        }
        CameraScript.Instance.SetupFightCamera();
    }
    
    
    private void LaunchAttack(BoardCharacter character)
    {
        GameObject newGameObject = new GameObject("Projectile");
        newGameObject.transform.localScale = new Vector3(0, 1, 0);
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = kikoha;
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.sortingOrder = 5;

        newGameObject.transform.position = character.gameObject.transform.position + new Vector3((startMargin.x * character.direction.x), startMargin.y);

        Vector3 direction = character.direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        List<GameObject> dangerTiles = new List<GameObject>(); // Declare the dangerTiles list
        List<Vector2Int> dangerTilesPositions = new List<Vector2Int>();

        for (int i = 1; i <= kikohaSize.x; i++)
        {
            for (int j = -(kikohaSize.y / 2); j <= (kikohaSize.y / 2); j++)
            {
                Vector3 offset = new Vector3(direction.y, -direction.x) * j;
                Vector3 tilePosition = character.gameObject.transform.position + direction * i + offset;

                GameObject dangerTile = new GameObject("DangerTile");
                dangerTile.transform.position = tilePosition;
                dangerTile.transform.localScale = new Vector3(0.9f, 0.9f, 1);
                SpriteRenderer tileSpriteRenderer = dangerTile.AddComponent<SpriteRenderer>();
                // tileSpriteRenderer.sprite = dangerTileSprite;
                tileSpriteRenderer.color = new Color(1f, 0f, 0f, 1f);
                tileSpriteRenderer.sortingOrder = 4;

                dangerTiles.Add(dangerTile); // Add to dangerTiles list
                dangerTilesPositions.Add(new Vector2Int(Mathf.FloorToInt(tilePosition.x), Mathf.FloorToInt(tilePosition.y)));
            }
        }

        newGameObject.LeanScale(new Vector3(1, 1, 0), 0.3f).setOnComplete(() =>
        {
            try {
                BoardUtils.InflictDamageInZone(dangerTilesPositions.ToArray(), character.character.GetAttackDamage() * attackMultiplicator); 
            } catch(Exception error){
                Debug.LogWarning("There is an error here, " + error);
            }
            LeanTween.value(newGameObject, f => spriteRenderer.color = new Color(1f, 1f, 1f, f), 1f, 0f, 0.3f).setOnComplete(() =>
            {
                foreach (var tile in dangerTiles)
                {
                    SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();
                    LeanTween.value(tile, a => tileRenderer.color = new Color(1f, 0f, 0f, a), 1f, 0f, 0.3f).setOnComplete(() =>
                    {
                        MonoBehaviour.Destroy(tile);
                    });
                }
                MonoBehaviour.Destroy(newGameObject);
            });
        });
    }

}