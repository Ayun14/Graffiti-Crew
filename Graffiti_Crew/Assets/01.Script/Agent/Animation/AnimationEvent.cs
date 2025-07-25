using System;
using UnityEngine;

public static class AnimationEvent
{
    public static Action<int, AnimationEnum> SetAnimation;

    public static Action<DialogueData> SetDialogueAnimation;
    public static Action EndDialogueAnimation;
    public static Action SetBubble;
}
