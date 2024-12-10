using UnityEngine;

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}

public static class CardRarityExtensions
{
    public static Color GetRarityColor(this CardRarity rarity)
    {
        switch (rarity)
        {
            case CardRarity.Common:
                return Color.white;
            case CardRarity.Uncommon:
                return new Color(0f, 1f, 1f, 1f);
            case CardRarity.Rare:
                return new Color(1f, 1f, 0.5f, 1f);
            case CardRarity.Epic:
                return new Color(1f, 0.5f, 1f, 1f);
            default:
                return Color.white;
        }
    }
}