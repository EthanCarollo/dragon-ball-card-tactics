using UnityEngine;

[CreateAssetMenu(fileName = "Fight", menuName = "Fight/Fight")]
public class Fight : ScriptableObject
{
        [SerializeField]
        public CharacterContainerFight[] opponents;
}