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

        private DropdownField _languageField;
        private LanguageType _lauguageType;

        private bool isLanguageChangeing;

        private LanguageSO _lauguageSO => ViewModel.GetLanguageSO();
        private string[] _lauguageTypes => _lauguageSO.languageTypes;

        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ViewModel = viewModel as HangOutViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _languageField = topElement.Q<DropdownField>("language-dropdownField");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
        }

        private void ChangeLanguage(ChangeEvent<string> evt) {
            if (isLanguageChangeing) {
                Debug.Log("cancel");
                isLanguageChangeing = false;
                return;
            }
            LanguageType inputValue = LanguageType.Korea;
            int index = -1;

            for (int i = 0; i < _lauguageTypes.Length; i++) {
                if (_lauguageTypes[i] == evt.newValue) {
                    LanguageType[] states = (LanguageType[])Enum.GetValues(typeof(LanguageType));
                    inputValue = states[i];
                    index = i;
                }
            }
            _lauguageType = inputValue;
            UIEvents.ChangeLanguageEvnet?.Invoke(_lauguageType);
            if (index > -1) {
                _languageField.SetValueWithoutNotify(_lauguageTypes[index]);
            }
            SetLanguageItems();
        }
        private void SetLanguageItems() {
            isLanguageChangeing = true;
            _languageField.label = _lauguageSO.title;
            _languageField.choices.Clear();
            _languageField.choices = _lauguageTypes.ToList();
        }
    }
}