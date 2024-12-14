using System;
using UnityEngine;

[Serializable]
public class PoisonEffect : Effect
{
    public new readonly string effectName = "Poison effect";
    public new readonly string effectDescription = "";

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