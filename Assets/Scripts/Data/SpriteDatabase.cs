using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "SpriteData/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{
    private static SpriteDatabase _instance;
    
    public BoardAnimation disappearAnimation;
    public BoardAnimation appearAnimation;
    public BasicTransformAnimation basicTransformAnimation;
    public Sprite basePassiveSprite;
    public Texture2D normalCursor;
    public Texture2D pointerCursor;
    public Sprite[] numbers;

    public Sprite fullfillHeart;
    public Sprite emptyHeart;

    public Sprite attackAbilityIcon;
    public Sprite kikohaAbilityIcon;
    public Sprite superKikohaAbilityIcon;
    public Sprite flashAttackAbilityIcon;
    public Sprite healingAbilityIcon;
    public Sprite healingOtherAbilityIcon;
    public Sprite transformAbilityIcon;
    public Sprite fusionTransformAbilityIcon;
    public Sprite distanceTopAttackAbilityIcon;
    public Sprite applyAllEffectAbilityIcon;
    public Sprite janembaAttackAbilityIcon;
    public Sprite kiChargingAbilityIcon;

    public static SpriteDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<SpriteDatabase>("SpriteDatabase");
                if (_instance == null)
                {
                    Debug.LogError("SpriteDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
