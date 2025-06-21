using System.Collections.Generic;
using UnityEditor;

public static class ProjectBuilder
{
    public static List<string> scenesToBuild = new List<string>() { "Assets/Demo/PlayerCharactersDemo.unity" };
    public static string projectName = "PlayerCharacters";

    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        // https://docs.unity3d.com/ScriptReference/BuildPipeline.html
        // https://docs.unity3d.com/530/Documentation/Manual/CommandLineArguments.html
        // https://docs.unity3d.com/ScriptReference/BuildTarget.html

        BuildPipeline.BuildPlayer(scenesToBuild.ToArray(), $"Builds/Windows/{projectName}.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}