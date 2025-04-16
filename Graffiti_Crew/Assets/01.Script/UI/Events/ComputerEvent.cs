using AH.UI.Data;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Events {
    public class ComputerEvent : MonoBehaviour {
        public static Action ShowStoreViewEvent;
        public static Action ShowSelectStageViewEvent;
        public static Action ShowStageDescriptionViewEvent;
        public static Action<bool> ActiveItemCountViewEvent;
        public static Action<Vector2, (ProductSO, VisualElement, int)> SetItemCountViewPosEvent;
        public static Action<(ProductSO, VisualElement, int)> BuyItemEvent;
        public static Action ShowNotEnoughViewEvent;
     
        public static Action HideViewEvent;

        public static Action<string, string> SelectStageEvent;
    }
}