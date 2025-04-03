using AH.UI.Events;
using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

public enum DialougeCharacter {
    Jia,
    Other
}
namespace AH.UI.Views {
    public class DialougeView : UIView {
        private DialogViewModel ViewModel;

        private VisualElement _otherDialouge;
        private VisualElement _jiaDialouge;
        private VisualElement _currentDialouge;
        private VisualElement _preDialouge;

        private VisualElement _profile;


        public DialougeView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ViewModel = viewModel as DialogViewModel;
            DialougeEvent.SetCharacterEvent += SetCharscter;
            DialougeEvent.ChangeCharacterEvent += ChangeCharacter;
        }

        public override void Dispose() {
            DialougeEvent.SetCharacterEvent -= SetCharscter;
            DialougeEvent.ChangeCharacterEvent -= ChangeCharacter;
            base.Dispose();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _otherDialouge = topElement.Q<VisualElement>("left-dialouge");
            _jiaDialouge = topElement.Q<VisualElement>("right-dialouge");

            _profile = topElement.Q<VisualElement>("profile");
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }

        private void SetCharscter(DialougeCharacter character) {
            switch (character) {
                case DialougeCharacter.Jia:
                    _currentDialouge = _jiaDialouge;
                    _preDialouge = _otherDialouge;
                    break;
                case DialougeCharacter.Other:
                    _currentDialouge = _otherDialouge;
                    _preDialouge = _jiaDialouge;
                    break;
            }
            Show(_currentDialouge, _preDialouge);
        }
        private void ChangeCharacter() {
            VisualElement emptyView = _currentDialouge;
            _currentDialouge = _preDialouge;
            _preDialouge = emptyView;

            Show(_currentDialouge, _preDialouge);
        }
        public virtual void Show(VisualElement showView, VisualElement hideView) {
            showView.style.display = DisplayStyle.Flex;

            hideView.style.display = DisplayStyle.None;
        }
    }
}