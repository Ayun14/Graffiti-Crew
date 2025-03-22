using System;
using UnityEngine;

public enum AnimationEnum
{
    Idle,
    Ready,
    Paint,
    Win,
    Lose,
    People_Idle,
    People_Clap
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
}
