using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDatabase", menuName = "SpriteData/SpriteDatabase")]
public class SpriteDatabase : ScriptableObject
{
    private static SpriteDatabase _instance;

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
