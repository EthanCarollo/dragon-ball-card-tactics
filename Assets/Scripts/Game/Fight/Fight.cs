using UnityEngine;

[CreateAssetMenu(fileName = "Fight", menuName = "Fight/Fight")]
public class Fight : ScriptableObject
{
        [SerializeField]
        public CharacterContainerFight[] opponents;
        public FightDifficulty difficulty = FightDifficulty.Easy;
}

public enum FightDifficulty
{
        Easy,
        Medium,
        Hard,
        Hardcore
}