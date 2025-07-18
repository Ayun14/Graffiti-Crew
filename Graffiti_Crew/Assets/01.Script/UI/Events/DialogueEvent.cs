using System;
using UnityEngine;

namespace AH.UI.Events {
    public class DialogueEvent : MonoBehaviour {
        public static Action<bool> ShowDialougeViewEvent;
        public static Action<bool> ShowMiniDialougeViewEvent;
        public static Action<bool> SetDialogueInput;

        public static Action<DialougeCharacter> SetDialogueEvent;
        public static Action ShowFeelingDialogueEvent;
    }
}