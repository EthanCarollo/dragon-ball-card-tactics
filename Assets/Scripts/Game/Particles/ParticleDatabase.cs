using UnityEngine;

[CreateAssetMenu(fileName = "NewParticleDatabase", menuName = "Particle/ParticleDatabase")]
public class ParticleDatabase : ScriptableObject
{
    private static ParticleDatabase _instance;
    
    public GameObject sparkParticlePrefab;

    public static ParticleDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ParticleDatabase>("ParticleDatabase");
                if (_instance == null)
                {
                    Debug.LogError("ParticleDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
