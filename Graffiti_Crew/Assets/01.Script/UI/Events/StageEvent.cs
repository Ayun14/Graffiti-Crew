using System;

namespace AH.UI.Events {
    public static class StageEvent {
        public static Action<bool> SetActiveFightViewEvent;
        public static Action<bool, bool> ShowResultViewEvent;

        public static Action<bool> ShowVictorScreenEvent;
        public static Action ChangeSprayValueEvent;

        public static Action HideViewEvent;
    }
}
