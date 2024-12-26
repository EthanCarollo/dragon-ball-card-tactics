public class PlayerMana{
    // The player start with a max amount of mana of 1
    // public int CurrentMana = 1; TODO same as player level lol
    public int CurrentMana = 6;
    public int MaxMana = 6;

    public void AddMana(int mana){
        CurrentMana+=mana;
        if(CurrentMana > MaxMana){
            CurrentMana = MaxMana;
        }
    }
}