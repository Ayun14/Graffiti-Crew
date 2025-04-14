using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ItemCountView : UIView {
        private Vector2 screenPos;
        public ItemCountView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ComputerEvent.SetItemCountViewPosEvent += SetPos;
        }
        public override void Dispose() {
            ComputerEvent.SetItemCountViewPosEvent -= SetPos;
            base.Dispose();
        }

        private void SetPos(Vector2 pos) {
            screenPos = pos;
            Debug.Log(pos);
        }

        public override void Show() {
            topElement.style.left = screenPos.x;
            topElement.style.top = screenPos.y;
            base.Show();
        }
    }
}
