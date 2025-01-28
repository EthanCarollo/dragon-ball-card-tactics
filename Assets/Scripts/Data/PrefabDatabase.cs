using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Prefab/PrefabDatabase")]
public class PrefabDatabase : ScriptableObject
{
    private static PrefabDatabase _instance;

    public GameObject annotationUiPrefab;

    public GameObject namekDefaultMap;

    public GameObject draggedCardPrefab;

    public GameObject cardDeckMainMenuPrefab;

    public static PrefabDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<PrefabDatabase>("PrefabDatabase");
                if (_instance == null)
                {
                    Debug.LogError("PrefabDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }        
}