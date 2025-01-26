using System.Collections;
using UnityEngine;

public class CoroutineSystem : MonoBehaviour{
    private static CoroutineSystem _instance;

    private static CoroutineSystem Instance {
        get {
            if (_instance == null) {
                // ���ο� GameObject�� CoroutineManager ������Ʈ �߰�
                var go = new GameObject("CoroutineSystem");
                _instance = go.AddComponent<CoroutineSystem>();

                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public static Coroutine RunCoroutine(IEnumerator coroutine) {
        return Instance.StartCoroutine(coroutine);
    }
    public static void StopCoroutineInstance(Coroutine coroutine) {
        if (coroutine != null) {
            Instance.StopCoroutine(coroutine);
        }
    }
    public static void StopAllCoroutinesInstance() {
        Instance.StopAllCoroutines();
    }
}
