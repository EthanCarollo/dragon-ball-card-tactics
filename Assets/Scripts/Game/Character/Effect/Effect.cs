using System;

[Serializable]
public abstract class Effect {
    public readonly string effectName = "";
    public readonly string effectDescription = "";

    public int attackBonus = 0;
    public float attackSpeedBonus = 0;

    public float effectDuration; // Durée totale de l'effet.
    public float tickInterval; // Temps entre chaque tick.
    public float nextTickTime; // Temps avant le prochain tick.

    public void UpdateEffect(float deltaTime, BoardCharacter character)
    {
        if (effectDuration <= 0) return;

        effectDuration -= deltaTime;
        nextTickTime -= deltaTime;

        if (nextTickTime <= 0)
        {
            OnEffectTick(character);
            nextTickTime = tickInterval;
        }
    }

    public abstract void OnEffectTick(BoardCharacter character);

    public bool IsEffectFinished()
    {
        return effectDuration <= 0;
    }

    public Effect Clone()
    {
        string serialized = UnityEngine.JsonUtility.ToJson(this);  // Serialize to JSON.
        return (Effect)UnityEngine.JsonUtility.FromJson(serialized, this.GetType());  // Deserialize back to a new instance.
    }
}