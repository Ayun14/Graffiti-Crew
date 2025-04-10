using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Profiling.HierarchyFrameDataView;

namespace AH.UI.Views {
    public enum LanguageType {
        Korea,
        English
    }
    public class SettingView : UIView {

        private Slider _bgmSlider;
        private Slider _vfxSlider;
        private DropdownField _languageField;

        private LanguageType _lauguageType;
        private Button _closeBtn;

        private int bgmValue;
        private int vfxValue;

        private bool isLanguageChangeing;
        private LanguageType[] _enumValues;
        private int _selectedIndex => viewModel.GetLanguageIndex();
        private LanguageSO _lauguageSO;
        private string[] _lauguageTypes;

        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            //_viewModel.GetBGMValue();
        }

        public override void Initialize() {
            _lauguageSO = viewModel.GetLanguageSO();
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
            HangOutEvent.SetPlayerMovementEvent?.Invoke(false);
            GameManager.SetPause(true);
            base.Show();
        }
        public override void Hide() {
            base.Hide();
            GameManager.SetPause(false);
            HangOutEvent.SetPlayerMovementEvent?.Invoke(true);
        }

        private void ClickCloseBtn(ClickEvent evt) {
            Debug.Log("click");
            HangOutEvent.HideViewEvent?.Invoke();
        }

        private void ChangeBgmValue(ChangeEvent<float> evt) {
            bgmValue = (int)evt.newValue;
            viewModel.SetBGMValue(bgmValue);
        }
        private void ChangeVfxValue(ChangeEvent<float> evt) {
            vfxValue = (int)evt.newValue;
            viewModel.SetVFXValue(vfxValue);
        }
        private void ClickResetSaveData(ClickEvent evt) {
            // 리셋 연결 안함
        }

        private void SetSound() {
            _bgmSlider.value = viewModel.GetBGMValue();
            _vfxSlider.value = viewModel.GetVFXValue();
        }
        private void ChangeLanguage(ChangeEvent<string> evt) {
            if (isLanguageChangeing) {
                isLanguageChangeing = false;
                return;
            }

            LanguageType inputValue = LanguageType.Korea;
            int index = _languageField.index;

            viewModel.SetLanguageIndex(index);
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