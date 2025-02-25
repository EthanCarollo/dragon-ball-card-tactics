
public class PlayerLife {
    public int CurrentLife = 1;
    public int MaxLife = 5;

    public void LooseLife(int life){
        CurrentLife -= life;
        if(CurrentLife <= 0){
            CurrentLife = 0;
        }
    }

    public bool IsAlive() => CurrentLife > 0;
}