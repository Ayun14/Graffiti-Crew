using System;

namespace AH.UI.Events {
    public static class FightEvent {
        public static Action ShowFightViewEvent;
        public static Action HideFightViewEvent;

        public static Action ShowResultViewEvent;

        public static Action<bool> SetActiveFightViewEvent;
    }
}
