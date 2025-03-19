using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
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

        private bool isTitle = false;

        private Slider _bgmSlider;
        private Slider _vfxSlider;
        private DropdownField _languageField;
        //private Button _resetSaveData;
        private LanguageType _lauguageType;
        private Button _closeBtn;

        private int bgmValue;
        private int vfxValue;

        private bool isLanguageChangeing;
        private LanguageType[] _enumValues;
        private int _selectedIndex => ViewModel.GetLanguageIndex();
        private LanguageSO _lauguageSO;
        private string[] _lauguageTypes;


        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            ViewModel.GetBGMValue();
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
            _bgmSlider = topElement.Q<Slider>("bgm-slider");
            _vfxSlider = topElement.Q<Slider>("vfx-slider");
            _languageField = topElement.Q<DropdownField>("language-dropdownField");
            _closeBtn = topElement.Q<Button>("close-btn");
            SetLanguageItems(false);
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _bgmSlider.RegisterValueChangedCallback(ChangeBgmValue);
            _vfxSlider.RegisterValueChangedCallback(ChangeVfxValue);
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _bgmSlider.UnregisterValueChangedCallback(ChangeBgmValue);
            _vfxSlider.UnregisterValueChangedCallback(ChangeVfxValue);
            _languageField.UnregisterValueChangedCallback(ChangeLanguage);
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
        }

        public override void Show() {
            SetSound();
            base.Show();
        }
        private void ClickCloseBtn(ClickEvent evt) {
            HangOutEvent.HideViewEvent?.Invoke();
        }

        private void ChangeBgmValue(ChangeEvent<float> evt) {
            bgmValue = (int)evt.newValue;
            ViewModel.SetBGMValue(bgmValue);
        }
        private void ChangeVfxValue(ChangeEvent<float> evt) {
            vfxValue = (int)evt.newValue;
            ViewModel.SetVFXValue(vfxValue);
        }
        private void ClickResetSaveData(ClickEvent evt) {
            // 리셋 연결 안함
        }

        private void SetSound() {
            _bgmSlider.value = ViewModel.GetBGMValue();
            _vfxSlider.value = ViewModel.GetVFXValue();
        }
        private void ChangeLanguage(ChangeEvent<string> evt) {
            if (isLanguageChangeing) {
                isLanguageChangeing = false;
                return;
            }

            LanguageType inputValue = LanguageType.Korea;
            int index = _languageField.index;

            ViewModel.SetLanguageIndex(index);
            inputValue = _enumValues[index];
            _lauguageType = inputValue;

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