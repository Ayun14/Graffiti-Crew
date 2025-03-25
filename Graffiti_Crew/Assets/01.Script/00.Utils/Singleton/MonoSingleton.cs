using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

    public static bool IsExist() => _instance != null;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindFirstObjectByType<T>();

            if (_instance == null)
            {
                Debug.LogWarning($"{typeof(T)} is not exist. Creating new instance.");
                _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            _instance.transform.SetParent(null);

            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
        if (_instance == this)
            _instance = null;
    }
}