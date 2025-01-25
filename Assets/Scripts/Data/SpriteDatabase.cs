using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "SpriteData/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{
    private static SpriteDatabase _instance;
    
    public BoardAnimation disappearAnimation;
    public BoardAnimation appearAnimation;
    public BoardAnimation basicTransformAnimation;
    public Sprite basePassiveSprite;
    public Texture2D normalCursor;
    public Texture2D pointerCursor;
    public Sprite[] numbers;

    public Sprite fullfillHeart;
    public Sprite emptyHeart;

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
