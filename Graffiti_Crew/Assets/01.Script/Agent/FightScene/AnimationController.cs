using System;
using UnityEngine;

public enum AnimationEnum
{
    // Fight
    Idle,
    Ready,
    Paint,
    Win,
    Lose,
    Run,
    People_Idle,
    People_Clap,

    // Dialogue
    Talk,
    Sit
}

public class AnimationController : MonoBehaviour
{
    [SerializeField] private AnimationEnum _defalutAnimation;

    private Animator _animator;
    private AnimationEnum _currentAnim;
    public int ObjectID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentAnim = _defalutAnimation;
    }

    private void Start()
    {
        if(ObjectID != -1)
            AnimationManager.Register(ObjectID, this);
    }

    private void OnDestroy()
    {
        AnimationManager.Unregister(ObjectID, this);
    }

    public void PlayAnimation(AnimationEnum anim)
    {
        ExitAnim();
        _animator.SetBool(anim.ToString(), true);
        _currentAnim = anim;
    }

    private void ExitAnim()
    {
        _animator.SetBool(_currentAnim.ToString(), false);
    }

    public void SetObjectID(int newID)
    {
        ObjectID = newID;
        AnimationManager.Register(ObjectID, this);
    }
}
