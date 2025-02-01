using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "HistoryDatabase", menuName = "History/HistoryDatabase")]
public class HistoryDatabase : ScriptableObject
{
    private static HistoryDatabase _instance;

    public HistoryFight[] history;


    public static HistoryDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<HistoryDatabase>("HistoryDatabase");
                if (_instance == null)
                {
                    Debug.LogError("HistoryDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    public void AddFight(CharacterContainer[] characters, int round, int seconds){
        var historyList = history.ToList();
        historyList.Add(new HistoryFight(characters, round, seconds));
        history = historyList.ToArray();
    }
}

[Serializable]
public class HistoryFight {
    public CharacterContainer[] characters;
    public int round;
    public int seconds;

    public HistoryFight(CharacterContainer[] characters, int round, int seconds){
        this.characters = characters;
        this.round = round;
        this.seconds = seconds;
    }
}