using UnityEngine;

public class ParticleManager : MonoBehaviour
{
        public static ParticleManager Instance;
        public void Awake()
        {
                // TODO : make this particle manager singleton better
                Instance = this;
        }

        public void InstantiateParticle(Vector3 position, GameObject prefab)
        {
                Instantiate(prefab, position, Quaternion.identity);
        }
}