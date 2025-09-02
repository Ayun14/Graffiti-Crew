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
    People_Pointing,
    People_Reaction,
    People_Talk,

    // Dialogue
    Talk,
    Talk2,
    Sit,
    Happy,
    Sad,
    Surprised,
    Thinking,
    Call,

    // Activity
    SprayNone,
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

        if (_animator == null)
        {
            Debug.LogWarning($"AnimationController on {gameObject.name} has no Animator component.", this);
        }

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
        if (_animator == null || !_animator.gameObject.activeInHierarchy)
        {
            return;
        }

        ExitAnim();
        _animator.SetBool(_animationHashes[anim], true);
        _currentAnim = anim;
    }

    private void ExitAnim()
    {
        if (_animator == null || !_animator.gameObject.activeInHierarchy)
        {
            return;
        }

        if (_animationHashes.ContainsKey(_currentAnim))
        {
            _animator.SetBool(_animationHashes[_currentAnim], false);
        }
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
