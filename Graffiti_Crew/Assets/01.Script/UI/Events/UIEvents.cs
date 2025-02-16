using AH.UI.Views;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Events {
    public static class UIEvents
    {
        public static Action<LanguageType> ChangeLanguageEvnet;
        public static Action CloseComputerEvnet;
    }
}