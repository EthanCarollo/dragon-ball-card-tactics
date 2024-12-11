
public class PlayerInfo {
    public PlayerLevel Level;
    public PlayerMana Mana;
    public PlayerLife Life;

    public PlayerInfo(){
        Level = new PlayerLevel();
        Mana = new PlayerMana();
        Life = new PlayerLife();
    }
}