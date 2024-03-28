/*
// Unity
using UnityEngine;

/// <summary>
/// Creates resources and marks them with don't destroy on load.
/// </summary>
/// <remarks>
/// Prefabs stored in Assets/Resources and listed in _ResourceNames will be instantiated on startup,
/// in whichever scene you're in. This ensures that manager dependencies are always available in an elegant way.
/// A detailed explanation can be viewed at https://www.youtube.com/watch?v=zJOxWmVveXU
/// </remarks>
/// <reference>
/// Code used in Module "Commercial Games Development" from video game "Project: Zip Zap".
/// </reference>
public static class Bootstrapper
{
    private static readonly string[] _ResourceNames =
    {
        "Managers"
    };

    /// <summary>
    /// This function is called once on startup and creates all prefabs listed in _ResourceNames
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        foreach (string resourceName in _ResourceNames)
        {
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load(resourceName)));
        }
    }
}
*/