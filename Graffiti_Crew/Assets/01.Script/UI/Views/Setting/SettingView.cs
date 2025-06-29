using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections;
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
        private  SettingViewModel _settingViewModel;

        private List<Button> _categoryBtnList = new List<Button>();
        private List<VisualElement> _categoryContentList = new List<VisualElement>();

        private Slider _bgmSlider;
        private Slider _sfxSlider;
        private int bgmValue;
        private int vfxValue;

        private DropdownField _languageField;
        private LanguageType _lauguageType;
        private bool isLanguageChangeing;
        private LanguageType[] _enumValues;
        private int _selectedIndex => _settingViewModel.GetLanguageIndex();
        private LanguageSO _lauguageSO;
        private string[] _lauguageTypes;

        private Button _goToTitleBtn;

        private Button _closeBtn;


        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            _settingViewModel = viewModel as SettingViewModel;

            _lauguageSO = _settingViewModel.GetLanguageSO();
            _lauguageTypes = _lauguageSO.languageTypes;
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            var categoryBtnContent = topElement.Q<VisualElement>("category-top-content");
            _categoryBtnList = categoryBtnContent.Query<Button>(className: "setting-category-btn").ToList();

            var categoryContent = topElement.Q<VisualElement>("catgory-content");
            _categoryContentList = categoryContent.Children().ToList();
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));

            _bgmSlider = topElement.Q<Slider>("bgm-slider");
            _sfxSlider = topElement.Q<Slider>("sfx-slider");

            _languageField = topElement.Q<DropdownField>("language-dropdownField");

            _goToTitleBtn = topElement.Q<Button>("title-btn");

            _closeBtn = topElement.Q<Button>("close-btn");

            //SetLanguageItems(false);
            ShowCategory(0);
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int categoryBtnIndex = 0;
            foreach (var button in _categoryBtnList) {
                button.RegisterCallback<ClickEvent, int>(ClickCategory, categoryBtnIndex++);
            }
            _bgmSlider.RegisterValueChangedCallback(ChangeBgmValue);
            _sfxSlider.RegisterValueChangedCallback(ChangeSfxValue);
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
            _goToTitleBtn.RegisterCallback<ClickEvent>(ClickGoToTitleBtn);
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _categoryBtnList) {
                button.UnregisterCallback<ClickEvent, int>(ClickCategory);
            }
            _bgmSlider.UnregisterValueChangedCallback(ChangeBgmValue);
            _sfxSlider.UnregisterValueChangedCallback(ChangeSfxValue);
            _languageField.UnregisterValueChangedCallback(ChangeLanguage);
            _goToTitleBtn.UnregisterCallback<ClickEvent>(ClickGoToTitleBtn);
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
        }

        public override void Show() {
            HangOutEvent.SetPlayerMovementEvent?.Invoke(false);
            GameManager.SetPause(true);
            SetSound();
            base.Show();
        }
        public override void Hide() {
            HangOutEvent.SetPlayerMovementEvent?.Invoke(true);
            base.Hide();
            GameManager.SetPause(false);
        }
        private void ClickCloseBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            StageEvent.HideViewEvent?.Invoke();
        }

        private void ClickCategory(ClickEvent evt, int index) {
            ShowCategory(index);
        }
        private void ShowCategory(int index) {
            foreach(VisualElement content in _categoryContentList) {
                content.style.display = DisplayStyle.None;
            }
            _categoryContentList[index].style.display = DisplayStyle.Flex;
        }

        #region Audio
        private void SetSound() {
            _bgmSlider.value = _settingViewModel.GetBGMValue();
            _sfxSlider.value = _settingViewModel.GetVFXValue();
        }
        private void ChangeBgmValue(ChangeEvent<float> evt) {
            bgmValue = (int)evt.newValue;
            _settingViewModel.SetBGMValue(bgmValue);
            GameEvents.BgmChangeEvnet?.Invoke();
        }
        private void ChangeSfxValue(ChangeEvent<float> evt) {
            vfxValue = (int)evt.newValue;
            _settingViewModel.SetSFXValue(vfxValue);
        }
        #endregion

        

        #region Language
        private void ChangeLanguage(ChangeEvent<string> evt) {
            if (isLanguageChangeing) {
                isLanguageChangeing = false;
                return;
            }

            LanguageType inputValue = LanguageType.Korea;
            int index = _languageField.index;

            _settingViewModel.SetLanguageIndex(index);
            inputValue = _enumValues[index];
            _lauguageType = inputValue;

            UIEvents.ChangeLanguageEvnet?.Invoke(_lauguageType);
            //SetLanguageItems(true);
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
        #endregion

        private void ClickGoToTitleBtn(ClickEvent evt) {
            GameManager.Instance.CharacterFade(1, 0);
            SaveDataEvents.SaveGameEvent?.Invoke("TitleScene");
        }
    }
}