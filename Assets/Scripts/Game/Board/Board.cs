using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Board : MonoBehaviour
{
    public GameObject[,] BoardArray;
    public CharacterContainer[,] CharacterContainerArray;
    
    public abstract void CreateBoard();
    public abstract bool AddCharacterFromBoard(BoardCharacter character, Vector2Int position);
    public abstract void RemoveCharacterFromBoard(BoardCharacter character);
}
