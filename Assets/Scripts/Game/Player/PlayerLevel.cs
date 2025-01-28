
public class PlayerLevel {
    public int CurrentLevel = 1;
    public int MaxLevel = 10;
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