using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

public class BuildScript
{
    public static void PerformBuild()
    {
        PlayerSettings.WebGL.template = "PROJECT:DragonBallCardTactics";
        PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, new[] { GraphicsDeviceType.OpenGLES3 });
        
        
        string buildPath = "Build/WebGL";
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            throw new BuildFailedException(
                "Impossible de construire le projet : aucune scène active n'est configurée dans les Build Settings.");
        }

        BuildReport report = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new BuildFailedException($"Échec du build WebGL : {report.summary.result}.");
        }

        Debug.Log($"Build WebGL terminé dans {buildPath} ({report.summary.totalSize} octets).");
    }
}
