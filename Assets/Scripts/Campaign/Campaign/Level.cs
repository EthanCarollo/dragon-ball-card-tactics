using UnityEngine;
using System;

// A campaign is just a composition of multiple Levels
[Serializable]
public class Level
{
    public CharacterInLevel[] characters;

    [SerializeField] public Dialog[] StartDialog;
}

[Serializable]
public class CharacterInLevel
{
    public CharacterData character;
    public Vector2Int position;
}