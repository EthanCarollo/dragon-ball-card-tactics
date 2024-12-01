using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    private static CardDatabase _instance;

    public Card[] cards;

    public Card[] playerCards;

    public static CardDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CardDatabase>("CardDatabase");
                if (_instance == null)
                {
                    Debug.LogError("ShadersDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
        
}