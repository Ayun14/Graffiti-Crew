using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionObject : MonoBehaviour
{
    public PlayerStateEnum stateEnum;

    public Vector3 TargetPos => _targetPos;
    private Vector3 _targetPos;

    protected virtual void Awake()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HangOutScene"))
        {
            _targetPos = transform.Find("Target").position;
        }
    }
}
