using UnityEngine;

public class PlayerMana{
    // The player start with an amount of mana of 1
    public int CurrentMana = 1;
    public int MaxMana = 6;

    public void AddMana(int mana){
        CurrentMana = Mathf.Clamp(CurrentMana + mana, 0, Mathf.Max(0, MaxMana));
    }
}
