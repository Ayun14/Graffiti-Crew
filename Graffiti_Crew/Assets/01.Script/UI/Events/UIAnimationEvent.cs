using System;
using UnityEngine;

public static class UIAnimationEvent {
    public static Action<bool> SetActiveStartAnimationEvnet; // 대결 시작 
    public static Action<bool> SetActiveEndAnimationEvnet; // 대결 시작 
    public static Action<bool> SetActiveRivalCheckAnimationEvnet; // 긴장감 연출
}
