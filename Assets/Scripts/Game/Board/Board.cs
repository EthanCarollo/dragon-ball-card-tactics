using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Board : MonoBehaviour
{
    public GameObject[,] BoardArray;
    public CharacterContainer[,] CharacterContainerArray;
    
    public abstract void CreateBoard(BoardObject[,] boardCharacterArray);
    public abstract bool AddCharacterFromBoard(BoardCharacter character, Vector2Int position);
}
