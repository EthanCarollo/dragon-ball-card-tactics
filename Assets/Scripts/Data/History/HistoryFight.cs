using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HistoryFight
{
    public CharacterContainer[] characters;
    
    [SerializeReference, SubclassSelector]
    public HistoryAction[] historyActions;
    
    public int round;
    public int seconds;

    public HistoryFight(CharacterContainer[] characters, int round, int seconds)
    {
        this.characters = characters;
        this.round = round;
        this.seconds = seconds;
    }
}