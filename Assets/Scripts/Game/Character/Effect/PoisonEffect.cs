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
        try
        {
            particle.StartParticle(character.gameObject.transform.position);
            character.HitDamage(damagePerTick);
        }
        catch (System.Exception error)
        {
            Debug.LogError("Error on effect tick poison effect : " + error.ToString());
        }
        Debug.Log($"[{effectName}] inflige {damagePerTick} dégâts à {character.character.GetName()}.");
    }
}