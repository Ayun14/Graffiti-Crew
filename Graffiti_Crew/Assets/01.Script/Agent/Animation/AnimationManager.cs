using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static Dictionary<int, List<AnimationController>> _animControllersByID = new();

    private void OnEnable()
    {
        AnimationEvent.SetAnimation += HandleAnimation;
    }

    private void OnDisable()
    {
        AnimationEvent.SetAnimation -= HandleAnimation;
    }

    public static void Register(int id, AnimationController controller)
    {
        if (!_animControllersByID.ContainsKey(id))
        {
            _animControllersByID[id] = new List<AnimationController>();
        }

        _animControllersByID[id].Add(controller);
    }

    public static void Unregister(int id, AnimationController controller)
    {
        if (_animControllersByID.ContainsKey(id))
        {
            _animControllersByID[id].Remove(controller);

            if (_animControllersByID[id].Count == 0)
            {
                _animControllersByID.Remove(id);
            }
        }
    }

    private static void HandleAnimation(int id, AnimationEnum anim)
    {
        if (_animControllersByID.ContainsKey(id))
        {
            foreach (var controller in _animControllersByID[id])
            {
                controller.PlayAnimation(anim);
            }
        }
    }
}
