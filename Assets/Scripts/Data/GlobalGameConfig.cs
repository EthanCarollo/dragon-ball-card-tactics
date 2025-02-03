using UnityEngine;

[CreateAssetMenu(fileName = "GlobalGameConfig", menuName = "GlobalConfig/GlobalGameConfig")]
public class GlobalGameConfig : ScriptableObject
{
    private static GlobalGameConfig _instance;
    
    public bool debug;
    
    // If you want to update version of the game, update it here
    public static string version{ get => "0.0.3"; }

    public static GlobalGameConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GlobalGameConfig>("GlobalGameConfig");
                if (_instance == null)
                {
                    Debug.LogError("GlobalGameConfig instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
