using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// The singleton instance overrides the current instance instead of destroying any new ones.
/// </summary>
/// <reference>
/// Code used in Module "Commercial Games Development" from video game "Project: Zip Zap".
/// </reference>
public abstract class SingletonInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}


/// <summary>
/// Basic singleton.
/// </summary>
/// <reference>
/// Code used in Module "Commercial Games Development" from video game "Project: Zip Zap".
/// </reference>
public abstract class Singleton<T> : SingletonInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        base.Awake();
    }
}
