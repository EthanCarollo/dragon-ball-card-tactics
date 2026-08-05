using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : ScriptableObject
{
    public int id;
    public int manaCost = 1;
    public CardRarity rarity = CardRarity.Common;
    public new string name;
    public Sprite image;
    public Sprite fullartImage;
    public string uiEffectPreset;

    public virtual string GetDescription() {
        return "";
    }

    public virtual bool CanUseCard() {
        if (GameManager.Instance == null || manaCost < 0 ||
            manaCost > GameManager.Instance.Player.Mana.CurrentMana){
            return false;
        }
        return true;
    }

    public void RegisterCardHistory() {
        if (GameManager.Instance == null)
        {
            return;
        }

        var historyAction = new PlayCardHistoryAction();
        historyAction.cardPlayedId = this.id;
        historyAction.time = Mathf.RoundToInt(GameManager.Instance.elapsedTime);
        GameManager.Instance.AddHistoryAction(historyAction);
    }

    public abstract string GetCardType();
    public abstract void UseCard();
    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
}
