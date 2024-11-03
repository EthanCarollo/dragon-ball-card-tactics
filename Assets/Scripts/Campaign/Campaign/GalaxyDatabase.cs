using UnityEngine;

[CreateAssetMenu(fileName = "GalaxyDatabase", menuName = "Galaxy/GalaxyDatabase")]
public class GalaxyDatabase: ScriptableObject
{
    private static GalaxyDatabase _instance;
        
    public Galaxy[] galaxies;

    public static GalaxyDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GalaxyDatabase>("GalaxyDatabase");
                if (_instance == null)
                {
                    Debug.LogError("CampaignDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}