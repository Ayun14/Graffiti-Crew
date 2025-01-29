using AH.UI.Events;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public enum LanguageType {
        Korea,
        English
    }
    public class SettingView : UIView {
        // 모델 받아야함
        private EnumField _languageField;
        private LanguageType _lauguageType;

        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _languageField = topElement.Q<EnumField>();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
        private void ChangeLanguage(ChangeEvent<Enum> evt) {
            if (evt.newValue is LanguageType input) {
                _lauguageType = input;
                UIEvents.ChangeLanguageEvnet?.Invoke(_lauguageType);
            }
        }
    }
}