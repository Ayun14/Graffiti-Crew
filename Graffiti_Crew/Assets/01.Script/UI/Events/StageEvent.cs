using System;

namespace AH.UI.Events {
    public static class StageEvent {
        public static Action<bool> SetActiveFightViewEvent;
        public static Action<bool> ShowResultViewEvent;

        public static Action<bool> ShowVictorScreenEvent;

        public static Action<bool> SetActiveStartAnimation;
    }
}
