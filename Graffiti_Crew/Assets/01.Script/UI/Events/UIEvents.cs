using AH.Save;
using AH.UI.Views;
using System;

namespace AH.UI.Events {
    public static class UIEvents
    {
        public static Action<LanguageType> ChangeLanguageEvnet;
        public static Action CloseComputerEvnet;
        public static Action<SlotSO> ChangeSlotEvent;
    }
}