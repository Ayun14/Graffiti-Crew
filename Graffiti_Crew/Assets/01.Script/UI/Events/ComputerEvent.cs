using System;
using UnityEngine;

namespace AH.UI.Events {
    public class ComputerEvent : MonoBehaviour {
        public static Action ShowStoreViewEvent;
        public static Action ShowSelectStageViewEvent;
        public static Action ShowSelectFriendViewEvent;
        public static Action ShowStageDescriptionViewEvent;
     
        public static Action HideViewEvent;
    }
}