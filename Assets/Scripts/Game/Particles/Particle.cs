using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewParticle", menuName = "Particle/Particle")]
public class Particle : ScriptableObject
{    
    public Sprite[] particleSprites;

    public void StartParticle(Vector2 position)
{
    for (int i = 0; i < 3; i++) 
    {
        var particleObject = new GameObject("Particle" + i);
        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        particleObject.transform.position = position + randomOffset + new Vector2(0, 0.5f);

        float randomScale = Random.Range(0.3f, 0.8f);
        particleObject.transform.localScale = new Vector3(randomScale, randomScale, 1);

        SpriteRenderer spriteRenderer = particleObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 3;

        float randomSpeed = Random.Range(0.05f, 0.15f);
        ParticleManager.Instance.StartCoroutine(AnimateParticle(particleObject, spriteRenderer, randomSpeed));
    }
}

private IEnumerator AnimateParticle(GameObject particleObject, SpriteRenderer spriteRenderer, float speed)
{
    foreach (Sprite particleSprite in particleSprites)
    {
        spriteRenderer.sprite = particleSprite;
        yield return new WaitForSeconds(speed);
    }
    Destroy(particleObject);
}

}
