using System;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryDatabase", menuName = "History/HistoryDatabase")]
public class HistoryDatabase : ScriptableObject
{
    private static HistoryDatabase _instance;
    private static string FilePath => Path.Combine(Application.persistentDataPath, "history.json");

    public HistoryFight[] history = new HistoryFight[0];

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
                    _instance = CreateInstance<HistoryDatabase>();
                }
                else
                {
                    _instance.LoadHistory();
                }
            }
            return _instance;
        }
    }

    public void AddFight(CharacterContainer[] characters, int round, int seconds)
    {
        var historyList = history.ToList();
        historyList.Add(new HistoryFight(characters, round, seconds));
        history = historyList.ToArray();
        SaveHistory();
    }

    public void SaveHistory()
    {
        try
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save history: {e.Message}");
        }
    }

    public void LoadHistory()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load history: {e.Message}");
            }
        }
    }
}
