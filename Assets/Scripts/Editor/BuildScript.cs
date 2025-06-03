using UnityEditor;
using UnityEngine;

public class BuildScript
{
    public static void PerformBuild()
    {
        string buildPath = "Build/WebGL";
        string[] scenes = new string[] {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/FightScene.unity"
        };

        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
        Debug.Log("Build WebGL terminé dans " + buildPath);
    }
}
