using System;

public static class GameEvents {
    public static Action<StageDataSO> SendFightGameResultEvent;
    public static Action<int> SendCurrentStarCountEvent;
    public static Action BgmChangeEvnet;
}
