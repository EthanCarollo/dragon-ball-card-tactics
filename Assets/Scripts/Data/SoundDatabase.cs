using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "SoundData/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    private static SoundDatabase _instance;
    
    public AudioClip hoverButtonSound;
    public AudioClip clickButtonSound;
    public AudioClip addCardInDeckSound;
    public AudioClip retireCardInDeckSound;
    public AudioClip teleportSound;

    public static SoundDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<SoundDatabase>("SoundDatabase");
                if (_instance == null)
                {
                    Debug.LogError("SoundDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

}