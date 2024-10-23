using UnityEngine;

[CreateAssetMenu(fileName = "NewParticleData", menuName = "Particle/ParticleData")]
public class ParticleData : ScriptableObject
{
    private static ParticleData _instance;
    
    public GameObject sparkParticlePrefab;

    public static ParticleData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ParticleData>("ParticleData");
                if (_instance == null)
                {
                    Debug.LogError("ParticleData instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}
