using System;

namespace AH.UI.Events {
    public static class StageEvent {
        public static Action<bool> SetActiveFightViewEvent;
        public static Action<bool, bool> ShowResultViewEvent;
        public static Action<bool> ShowVictorScreenEvent;

        public static Action<bool> SetProgressEvnet; // true : fight, false : activity
        public static Action<bool> SetsprayCountEvnet; // true : fight, false : activity
        public static Action ChangeSprayValueEvent;
        public static Action ChangeGameProgressValueEvent;

        public static Action ClickNectBtnEvent;

        public static Action HideViewEvent;
    }
}
