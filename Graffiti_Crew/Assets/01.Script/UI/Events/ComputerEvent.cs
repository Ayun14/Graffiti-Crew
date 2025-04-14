using System;
using UnityEngine;

namespace AH.UI.Events {
    public class ComputerEvent : MonoBehaviour {
        public static Action ShowStoreViewEvent;
        public static Action ShowSelectStageViewEvent;
        public static Action ShowStageDescriptionViewEvent;
        public static Action<Vector2> ShowItemCountViewEvent;
        public static Action<Vector2> SetItemCountViewPosEvent;
     
        public static Action HideViewEvent;

        public static Action<string, string> SelectStageEvent;
    }
}