using UnityEngine.UIElements;

namespace AH.UI {
    public class TestView : UIView {
        private Button _slot1Btn;
        private Button _slot2Btn;
        private Button _slot3Btn;

        public TestView(VisualElement topContainer) : base(topContainer) {
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();

            _slot1Btn = topElement.Q<Button>("slot1-btn");
            _slot2Btn = topElement.Q<Button>("slot2-btn");
            _slot3Btn = topElement.Q<Button>("slot3-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

            _slot1Btn.RegisterCallback<ClickEvent>(ClickSlot1);
            _slot2Btn.RegisterCallback<ClickEvent>(ClickSlot2);
            _slot3Btn.RegisterCallback<ClickEvent>(ClickSlot3);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();

            _slot1Btn.UnregisterCallback<ClickEvent>(ClickSlot1);
            _slot2Btn.UnregisterCallback<ClickEvent>(ClickSlot2);
            _slot3Btn.UnregisterCallback<ClickEvent>(ClickSlot3);
        }

        private void ClickSlot1(ClickEvent evt) {
            Events.SelectSlotEvent?.Invoke("1");
        }
        private void ClickSlot2(ClickEvent evt) {
            Events.SelectSlotEvent?.Invoke("2");
        }
        private void ClickSlot3(ClickEvent evt) {
            Events.SelectSlotEvent?.Invoke("3");
        }
    }
}
