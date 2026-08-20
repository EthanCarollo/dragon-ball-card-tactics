using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Effect/PoisonEffect")]
public class PoisonEffect : Effect
{
    [SerializeField]
    private int damagePerTick;
    public Particle particle;

    public override void OnEffectTick(BoardCharacter character)
    {
        if (character?.character == null)
        {
            return;
        }

        try
        {
            if (particle != null && character.gameObject != null)
            {
                particle.StartParticle(character.gameObject.transform.position);
            }
        }
        catch (System.Exception error)
        {
            Debug.LogError("Error on effect tick poison effect : " + error.ToString());
        }

        character.HitDamage(damagePerTick);
        Debug.Log($"[{effectName}] inflige {damagePerTick} dégâts à {character.character.GetName()}.");
    }
}
