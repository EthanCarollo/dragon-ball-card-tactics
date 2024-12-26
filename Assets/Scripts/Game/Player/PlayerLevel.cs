
public class PlayerLevel {
    // public int CurrentLevel = 1; TODO reput that to 1 and make a normal debug system plz
    public int CurrentLevel = 4;
    public int MaxLevel = 6;
    public int CurrentExperience = 0;
    public int MaxExperience = 4;
    public int maxUnit = 4;

    public void AddExperience(int experience){
        if(MaxLevel == CurrentLevel){
            CurrentExperience = 100;
            MaxExperience = 100;
            return;
        }
        CurrentExperience += experience;
        while(CurrentExperience > MaxExperience){
            if(MaxLevel == CurrentLevel){
                CurrentExperience = 100;
                MaxExperience = 100;
                break;
            }
            CurrentLevel++;
            CurrentExperience -= MaxExperience;
            MaxExperience += 2 + (MaxExperience / 4);
            maxUnit++;
        }
    }
}