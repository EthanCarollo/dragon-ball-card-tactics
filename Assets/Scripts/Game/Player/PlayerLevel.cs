
public class PlayerLevel {
    public int CurrentLevel = 1;
    public int MaxLevel = 10;
    public int CurrentExperience = 0;
    public int MaxExperience = 4;
    public int maxUnit = 4;

    public void AddExperience(int experience){
        if (experience <= 0 || CurrentLevel >= MaxLevel){
            CurrentExperience = 100;
            MaxExperience = 100;
            return;
        }

        CurrentExperience += experience;

        while(CurrentExperience >= MaxExperience && CurrentLevel < MaxLevel){
            CurrentLevel++;
            CurrentExperience -= MaxExperience;
            MaxExperience += 2 + (MaxExperience / 4);
            maxUnit++;
        }

        if (CurrentLevel >= MaxLevel)
        {
            CurrentExperience = 100;
            MaxExperience = 100;
        }
    }
}
