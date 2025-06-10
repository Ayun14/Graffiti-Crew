using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationEnum
{
    // Fight
    Idle,
    Walk,
    Ready,
    Paint,
    Win,
    Lose,
    Run,
    People_Idle,
    People_Clap,

    // Dialogue
    Talk,
    Sit,
    Happy,
    Sad,
    Surprised,
    Thinking
}

public class AnimationController : MonoBehaviour
{
    [SerializeField] private AnimationEnum _defaultAnimation;

    private Animator _animator;
    private AnimationEnum _currentAnim;
    public int ObjectID;

    private static readonly Dictionary<AnimationEnum, int> _animationHashes = new Dictionary<AnimationEnum, int>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentAnim = _defaultAnimation;

        if (_animationHashes.Count == 0)
        {
            foreach (AnimationEnum anim in Enum.GetValues(typeof(AnimationEnum)))
            {
                _animationHashes[anim] = Animator.StringToHash(anim.ToString());
            }
        }
    }

    private void Start()
    {
        if (ObjectID != -1)
            AnimationManager.Register(ObjectID, this);
    }

    private void OnDestroy()
    {
        if (ObjectID != -1)
            AnimationManager.Unregister(ObjectID, this);
    }

    public void PlayAnimation(AnimationEnum anim)
    {
        ExitAnim();
        _animator.SetBool(_animationHashes[anim], true);
        _currentAnim = anim;
    }

    private void ExitAnim()
    {
        _animator.SetBool(_animationHashes[_currentAnim], false);
    }

    public void SetObjectID(int newID)
    {
        if (ObjectID != -1)
        {
            AnimationManager.Unregister(ObjectID, this);
        }

        ObjectID = newID;
        if (ObjectID != -1)
        {
            AnimationManager.Register(ObjectID, this);
        }
    }
}
