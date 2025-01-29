
using UnityEngine;

public class PlayerInfo {
    public PlayerLevel Level;
    public PlayerMana Mana;
    public PlayerLife Life;

    public PlayerInfo() {
        Level = new PlayerLevel();
        Mana = new PlayerMana();
        Life = new PlayerLife();

        if(GlobalGameConfig.Instance.debug == true) {
            Debug.Log("Debug Mode is active, setting MaxMana to 40 and CurrentMana to MaxMana, set PlayerLevel to 4 and set MaxUnit to 100.");
            Mana.MaxMana = 40;
            Mana.CurrentMana = Mana.MaxMana;
            Level.CurrentLevel = 6;
            Level.maxUnit = 100;
            Life.CurrentLife = 1;
        }
    }
}