using System;
using UnityEngine;

public abstract class Effect : ScriptableObject {
    public string effectName = "";
    public string effectDescription = "";
    public Sprite effectSprite;

    public int attackBonus = 0;
    public float attackSpeedBonus = 0;

    public float totalEffectDuration; // Durée totale de l'effet.
    public float tickInterval; // Temps entre chaque tick.

    public abstract void OnEffectTick(BoardCharacter character);
}

public class InGameEffect{
    public Effect effect;

    public float effectDuration; // Durée totale de l'effet.
    public float tickInterval; // Temps entre chaque tick.
    public float nextTickTime; // Temps avant le prochain tick.

    public InGameEffect(Effect effect){
        this.effect = effect;
        effectDuration = effect.totalEffectDuration;
        tickInterval = effect.tickInterval;
        nextTickTime = effect.tickInterval;
    }

    public void UpdateEffect(float deltaTime, BoardCharacter character)
    {
        if (effectDuration <= 0) return;

        effectDuration -= deltaTime;
        nextTickTime -= deltaTime;

        if (nextTickTime <= 0)
        {
            effect.OnEffectTick(character);
            nextTickTime = tickInterval;
        }
    }

    public bool IsEffectFinished()
    {
        return effectDuration <= 0;
    }
}