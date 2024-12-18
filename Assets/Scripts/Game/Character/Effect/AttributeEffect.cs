using System;

[Serializable]
public class AttributeEffect : Effect
{
    public new readonly string effectName = "Attribute effect";
    public new readonly string effectDescription = "";

    public override void OnEffectTick(BoardCharacter character)
    {

    }
}