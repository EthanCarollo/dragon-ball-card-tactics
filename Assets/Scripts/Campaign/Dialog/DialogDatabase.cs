using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDatabase", menuName = "Dialog System/DialogDatabase")]
public class DialogDatabase : ScriptableObject
{
    public Dialog[] startDialogs;

    private static DialogDatabase _instance;

    public static DialogDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DialogDatabase>("DialogDatabase");
                if (_instance == null)
                {
                    Debug.LogError("DialogDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }
}