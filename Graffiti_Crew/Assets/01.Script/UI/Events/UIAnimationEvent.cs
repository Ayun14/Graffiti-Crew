using System;
using UnityEngine;

public static class UIAnimationEvent {
    public static Action<bool> SetActiveStartAnimationEvnet; // ��� ���� 
    public static Action<bool> SetActiveEndAnimationEvnet; // ��� �� 
    public static Action<bool> SetActiveRivalCheckAnimationEvnet; // ���尨 ����
    public static Action<bool> SetActiveCountDownAnimationEvnet; // ī��Ʈ �ٿ�
    public static Action<bool> SetFilmDirectingEvent; // ��ȭ ����

    public static Action<Color> SetPlayerBackgroundColor;
    public static Action<Color> SetRivalBackgroundColor;
}
