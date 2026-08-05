using System;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryDatabase", menuName = "History/HistoryDatabase")]
public class HistoryDatabase : ScriptableObject
{
    private const int CurrentSaveVersion = 1;
    private static HistoryDatabase _instance;
    private static string FilePath => Path.Combine(Application.persistentDataPath, "history.json");
    private static string TemporaryFilePath => FilePath + ".tmp";
    private static string BackupFilePath => FilePath + ".bak";

    public HistoryFight[] history = new HistoryFight[0];

    [Serializable]
    private sealed class HistorySaveData
    {
        public int version;
        public HistoryFight[] history;
    }

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

    public void AddFight(CharacterContainer[] characters, int round, int seconds, HistoryAction[] historyActions)
    {
        var historyList = (history ?? Array.Empty<HistoryFight>())
            .Where(savedFight => savedFight != null)
            .ToList();
        HistoryFight historyFight = new HistoryFight(
            (characters ?? Array.Empty<CharacterContainer>())
                .Where(character => character != null)
                .Select(character => character.Clone())
                .ToArray(),
            Mathf.Max(0, round),
            Mathf.Max(0, seconds));
        historyFight.historyActions = historyActions ?? Array.Empty<HistoryAction>();
        historyList.Add(historyFight);
        history = historyList.ToArray();
        SaveHistory();
    }

    public void SaveHistory()
    {
        string json = JsonUtility.ToJson(new HistorySaveData
        {
            version = CurrentSaveVersion,
            history = history ?? Array.Empty<HistoryFight>()
        }, true);

        try
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(TemporaryFilePath, json);

            if (File.Exists(FilePath))
            {
                File.Replace(TemporaryFilePath, FilePath, BackupFilePath, true);
            }
            else
            {
                File.Move(TemporaryFilePath, FilePath);
            }
        }
        catch (Exception e)
        {
            TryDeleteTemporaryFile();
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
                var saveData = JsonUtility.FromJson<HistorySaveData>(json);
                if (saveData == null || saveData.history == null)
                {
                    throw new InvalidDataException("History save data is empty or invalid.");
                }

                if (saveData.version > CurrentSaveVersion)
                {
                    Debug.LogWarning(
                        $"History save uses a newer version ({saveData.version}) than supported ({CurrentSaveVersion}).");
                }

                history = saveData.history
                    .Where(savedFight => savedFight != null)
                    .Select(NormalizeHistoryFight)
                    .ToArray();
            }
            catch (Exception e)
            {
                BackupCorruptedFile();
                history = Array.Empty<HistoryFight>();
                Debug.LogError($"Failed to load history: {e.Message}");
            }
        }
    }

    private static HistoryFight NormalizeHistoryFight(HistoryFight savedFight)
    {
        savedFight.characters ??= Array.Empty<CharacterContainer>();
        savedFight.historyActions ??= Array.Empty<HistoryAction>();
        return savedFight;
    }

    private static void BackupCorruptedFile()
    {
        try
        {
            string backupPath = FilePath + $".corrupt-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            File.Move(FilePath, backupPath);
        }
        catch (Exception backupException)
        {
            Debug.LogWarning($"Failed to backup corrupted history: {backupException.Message}");
        }
    }

    private static void TryDeleteTemporaryFile()
    {
        try
        {
            if (File.Exists(TemporaryFilePath))
            {
                File.Delete(TemporaryFilePath);
            }
        }
        catch (Exception cleanupException)
        {
            Debug.LogWarning($"Failed to clean temporary history file: {cleanupException.Message}");
        }
    }
}
