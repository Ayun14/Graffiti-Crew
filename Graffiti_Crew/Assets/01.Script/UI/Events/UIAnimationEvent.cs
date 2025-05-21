using System;
using UnityEngine;

public static class UIAnimationEvent {
    public static Action<bool> SetActiveStartAnimationEvnet; // 대결 시작 
    public static Action<bool> SetActiveEndAnimationEvnet; // 대결 끝 
    public static Action<bool> SetActiveRivalCheckAnimationEvnet; // 긴장감 연출
    public static Action<bool> SetActiveCountDownAnimationEvnet; // 카운트 다운
    public static Action<bool> SetFilmDirectingEvent; // 영화 연출

    public static Action<Color> SetPlayerBackgroundColor;
    public static Action<Color> SetRivalBackgroundColor;
}
