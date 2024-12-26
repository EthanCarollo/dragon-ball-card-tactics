using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Board assignedBoard;
    public Vector2Int position;
    public Color baseColor;
    
    public void Start()
    {
        baseColor=GetComponentInChildren<SpriteRenderer>().color;
    }
    
    public void Update()
    {
        if(CharacterDragInfo.draggedObject != null && CharacterDragInfo.canPlayOnBoardPosition.x != -1 && 
            CharacterDragInfo.canPlayOnBoardPosition == position)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.2f);
        }
        else if (CharacterDragInfo.draggedObject != null && position.x <= 4 && CharacterDragInfo.canPlayOnBoardPosition.x == -1)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.2f);
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = baseColor;
        }
    }
}
