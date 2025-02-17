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
        private LanguageType[] _enumValues;
        private int _selectedIndex => ViewModel.GetLanguageIndex();
        private LanguageSO _lauguageSO;
        private string[] _lauguageTypes;

        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ViewModel = viewModel as HangOutViewModel;
            
            _lauguageSO = ViewModel.GetLanguageSO();
            _lauguageTypes = _lauguageSO.languageTypes;
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));

            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));
            _languageField = topElement.Q<DropdownField>("language-dropdownField");
            SetLanguageItems(false);
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
                isLanguageChangeing = false;
                Debug.Log("cancel");
                return;
            }

            LanguageType inputValue = LanguageType.Korea;
            int index = _languageField.index;

            ViewModel.SetLanguageIndex(index);
            inputValue = _enumValues[index];
            _lauguageType = inputValue;
            Debug.Log(_lauguageType);
            UIEvents.ChangeLanguageEvnet?.Invoke(_lauguageType);
            SetLanguageItems(true);
        }
        private void SetLanguageItems(bool active) {
            if (active) {
                isLanguageChangeing = true;
            }
            _languageField.SetValueWithoutNotify(_lauguageTypes[_selectedIndex]);
            _languageField.label = _lauguageSO.title;
            _languageField.choices.Clear();
            _languageField.choices = _lauguageTypes.ToList();
        }
    }
}