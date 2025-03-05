using System;

namespace AH.UI.Events {
    public static class FightEvent {
        public static Action ShowFightViewEvent;
        public static Action HideFightViewEvent;

        public static Action<bool> ShowResultViewEvent;
        public static Action<bool> GameResultEvent; // 사용안함 // true : win, false : lose
        public static Action VictorFullScreenEvent; // 사용안함
        public static Action<bool> ShowVictorScreenEvent;

        public static Action<bool> SetActiveFightViewEvent;
    }
}
