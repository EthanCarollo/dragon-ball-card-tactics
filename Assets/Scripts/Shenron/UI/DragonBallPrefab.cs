using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragonBallPrefab : MonoBehaviour, IPointerClickHandler
{
        public DragonBall prefabDragonBall;
        public Image dragonBallImage;

        public void SetupDragonBall(DragonBall dragonBall)
        {
                prefabDragonBall = dragonBall;
                dragonBallImage.sprite = dragonBall.dragonBallSprite;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
                DragonBallPanel.Instance.SelectDragonBall(prefabDragonBall);
        }
}