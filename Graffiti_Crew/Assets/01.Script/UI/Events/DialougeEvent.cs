using System;
using UnityEngine;

namespace AH.UI.Events {
    public class DialougeEvent : MonoBehaviour {
        public static Action<bool> ShowDialougeViewEvent;
        public static Action<bool> ShowMiniDialougeViewEvent;
    }
}