using AH.UI.Data;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ItemCountView : UIView {
        private Vector2 screenPos;
        private VisualElement view;

        private IntegerField _itemCountfield;
        private Button _buyBtn;

        private (ProductSO, VisualElement, int) itemData;

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

        protected override void SetVisualElements() {
            base.SetVisualElements();
            view = topElement.Q<VisualElement>("item-count-container");
            _buyBtn = topElement.Q<Button>("buy-btn");
            _itemCountfield = topElement.Q<IntegerField>("item-count-field");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _buyBtn.RegisterCallback<ClickEvent>(BuyItem);
        }

        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _buyBtn.UnregisterCallback<ClickEvent>(BuyItem);
        }

        private void BuyItem(ClickEvent evt) {
            itemData.Item3 = _itemCountfield.value;
            ComputerEvent.BuyItemEvent?.Invoke(itemData);
        }

        private void SetPos(Vector2 pos, (ProductSO, VisualElement, int) data) {
            screenPos = pos;
            itemData = data;
        }
        public override void Show() {
            view.style.left = screenPos.x;
            view.style.top = screenPos.y;
            base.Show();
        }
    }
}
