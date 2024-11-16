using UnityEngine;
using System;
using System.Collections.Generic;

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
    public List<int> unlockPassive;
    public Vector2Int position;
}