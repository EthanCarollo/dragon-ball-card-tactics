using System;
using UnityEngine;

public abstract class Effect : ScriptableObject {
    public string effectName;
    public string effectDescription;
    public Sprite effectSprite;

    public int attackBonus = 0;
    public float attackSpeedBonus = 0;

    public float totalEffectDuration; // Durée totale de l'effet.
    public float tickInterval; // Temps entre chaque tick.

    public virtual void OnEffectTick(BoardCharacter character)
    {
        return;
    }
}

[Serializable]
public class InGameEffect{
    public Effect effect;

    public float effectDuration; // Durée totale de l'effet.
    public float tickInterval; // Temps entre chaque tick.
    public float nextTickTime; // Temps avant le prochain tick.

    public InGameEffect(Effect effect){
        this.effect = effect;
        if (effect == null)
        {
            return;
        }

        effectDuration = Mathf.Max(0f, effect.totalEffectDuration);
        tickInterval = Mathf.Max(0.01f, effect.tickInterval);
        nextTickTime = tickInterval;
    }

    public InGameEffect Clone()
    {
        return new InGameEffect(effect)
        {
            effectDuration = effectDuration,
            tickInterval = tickInterval,
            nextTickTime = nextTickTime
        };
    }

    public void UpdateEffect(float deltaTime, BoardCharacter character)
    {
        if (effect == null || effectDuration <= 0) return;

        effectDuration -= deltaTime;
        nextTickTime -= deltaTime;

        if (nextTickTime <= 0)
        {
            effect.OnEffectTick(character);
            nextTickTime = Mathf.Max(0.01f, tickInterval);
        }
    }

    public bool IsEffectFinished()
    {
        return effectDuration <= 0;
    }
}
