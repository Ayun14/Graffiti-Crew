using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public enum LanguageType {
        Korea,
        English
    }
    public class SettingView : UIView {
        private HangOutViewModel ViewModel;
        // 모델 받아야함
        private DropdownField _languageField;
        private LanguageType _lauguageType;

        private string[] _lauguageTypes => ViewModel.GetLanguageTypes();

        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ViewModel = viewModel as HangOutViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _languageField = topElement.Q<DropdownField>("language-dropdownField");
            SetLanguageItems();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _languageField.RegisterCallback<ChangeEvent<string>>(ChangeLanguage);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _languageField.UnregisterCallback<ChangeEvent<string>>(ChangeLanguage);
        }
        private void ChangeLanguage(ChangeEvent<string> evt) {
            LanguageType inputValue = LanguageType.Korea;

            for (int i = 0; i < _lauguageTypes.Length; i++) {
                if (_lauguageTypes[i] == evt.newValue) {
                    LanguageType[] states = (LanguageType[])Enum.GetValues(typeof(LanguageType));
                    inputValue = states[i];
                }
            }
            _lauguageType = inputValue;
            UIEvents.ChangeLanguageEvnet?.Invoke(_lauguageType);

            SetLanguageItems();
        }
        private void SetLanguageItems() {
            _languageField.choices.Clear();
            _languageField.choices = _lauguageTypes.ToList();
        }
    }
}