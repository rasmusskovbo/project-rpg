using System.Security.Cryptography;
using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        T[] allObjectsOfType = FindObjectsOfType<T>();
        if (allObjectsOfType.Length > 1)
        {
            if (!allObjectsOfType[1].enabled)
            {
                Destroy(allObjectsOfType[1].gameObject);
            }
            else if (!allObjectsOfType[0].enabled)
            {
                Destroy(allObjectsOfType[0].gameObject);
            }
        }

        base.Awake();
    }
}
