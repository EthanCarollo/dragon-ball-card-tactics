using UnityEngine;

[CreateAssetMenu(fileName = "DragonBall", menuName = "DragonBall/DragonBall")]
public class DragonBall : ScriptableObject
{
    public string name;
    public int count;
    public int neededCount = 7;
    public Sprite dragonBallSprite;
    public DragonBallPossibility[] possibilities;
}