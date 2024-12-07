using UnityEngine;

[CreateAssetMenu(fileName = "ShadersDatabase", menuName = "Shaders/ShadersDatabase")]
public class ShadersDatabase : ScriptableObject
{
    private static ShadersDatabase _instance;
    
    public Material disappearMaterial;
    public Material outlineMaterial;
    public Material spriteMaterial;

    public static ShadersDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ShadersDatabase>("ShadersDatabase");
                if (_instance == null)
                {
                    Debug.LogError("ShadersDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
