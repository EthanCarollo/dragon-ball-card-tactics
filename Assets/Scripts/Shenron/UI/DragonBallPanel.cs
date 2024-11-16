using System;
using UnityEngine;

public class DragonBallPanel : MonoBehaviour
{
        public static DragonBallPanel Instance;
        
        public Transform dragonBallContainer;
        public GameObject dragonBallPrefab;
        public DragonBall selectedDragonBall;
        
        public void Awake()
        {
                Instance = this;
        }

        public void Start()
        {
                SetDragonBallPrefab();
        }

        public void SetDragonBallPrefab()
        {
                foreach (Transform child in dragonBallContainer)
                {
                        Destroy(child);
                }

                var dragonBallDatas = DragonBallDatabase.Instance.dragonBallDatas;

                foreach (var dragonBall in dragonBallDatas)
                {
                        var go = Instantiate(dragonBallPrefab, dragonBallContainer);
                        go.GetComponent<DragonBallPrefab>().SetupDragonBall(dragonBall);
                }
                
        }

        public void SelectDragonBall(DragonBall dragonBall)
        {
                selectedDragonBall = dragonBall;
                
        }
}