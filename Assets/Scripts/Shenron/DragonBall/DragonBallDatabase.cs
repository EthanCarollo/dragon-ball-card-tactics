using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DragonBallDatabase", menuName = "DragonBall/DragonBallDatabase")]
public class DragonBallDatabase : ScriptableObject
{
    private static DragonBallDatabase _instance;
    
    public DragonBall[] dragonBallDatas;

    public void Reset()
    {
        foreach (var dragonBall in dragonBallDatas)
        {
            dragonBall.count = 0;
        }
    }

    public static DragonBallDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DragonBallDatabase>("DragonBallDatabase");
                if (_instance == null)
                {
                    Debug.LogError("DragonBallDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}