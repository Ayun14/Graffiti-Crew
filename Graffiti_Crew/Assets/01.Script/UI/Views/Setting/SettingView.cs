using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections;
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

        private Slider _bgmSlider;
        private Slider _vfxSlider;
        private int bgmValue;
        private int vfxValue;
        
        private DropdownField _saveSlotField;
        private SlotSO[] slots;
        private string slotPath = "UI/Setting/Slots/";


        private DropdownField _languageField;
        private LanguageType _lauguageType;
        private bool isLanguageChangeing;
        private LanguageType[] _enumValues;
        private int _selectedIndex => _settingViewModel.GetLanguageIndex();
        private LanguageSO _lauguageSO;
        private string[] _lauguageTypes;

        private Button _closeBtn;


        public SettingView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            _settingViewModel = viewModel as SettingViewModel;

            slots = Resources.LoadAll<SlotSO>(slotPath);
            _lauguageSO = _settingViewModel.GetLanguageSO();
            _lauguageTypes = _lauguageSO.languageTypes;
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _enumValues = (LanguageType[])Enum.GetValues(typeof(LanguageType));
            _bgmSlider = topElement.Q<Slider>("bgm-slider");
            _vfxSlider = topElement.Q<Slider>("vfx-slider");
            _saveSlotField = topElement.Q<DropdownField>("saveSlot-dropdownField");
            _languageField = topElement.Q<DropdownField>("language-dropdownField");
            _closeBtn = topElement.Q<Button>("close-btn");

            _saveSlotField.index = _settingViewModel.GetSlotIndex();
            //SetLanguageItems(false);


            _saveSlotField.RegisterCallback<PointerDownEvent>(evt => {
#if UNITY_EDITOR
                // 에디터에서만 지연 호출 사용
                UnityEditor.EditorApplication.delayCall += () => {
                    //StyleDropdownItems();
                };
#else
        // 빌드 환경에서는 다음 프레임에서 실행하기 위해 코루틴 사용
        StartCoroutine(StyleDropdownItemsNextFrame());
#endif
            });
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _bgmSlider.RegisterValueChangedCallback(ChangeBgmValue);
            _vfxSlider.RegisterValueChangedCallback(ChangeVfxValue);
            _saveSlotField.RegisterValueChangedCallback(ChangeSlot);
            _languageField.RegisterValueChangedCallback(ChangeLanguage);
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _bgmSlider.UnregisterValueChangedCallback(ChangeBgmValue);
            _vfxSlider.UnregisterValueChangedCallback(ChangeVfxValue);
            _saveSlotField.UnregisterValueChangedCallback(ChangeSlot);
            _languageField.UnregisterValueChangedCallback(ChangeLanguage);
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

        #region Audio
        private void SetSound() {
            _bgmSlider.value = _settingViewModel.GetBGMValue();
            _vfxSlider.value = _settingViewModel.GetVFXValue();
        }
        private void ChangeBgmValue(ChangeEvent<float> evt) {
            bgmValue = (int)evt.newValue;
            _settingViewModel.SetBGMValue(bgmValue);
            GameEvents.BgmChangeEvnet?.Invoke();
        }
        private void ChangeVfxValue(ChangeEvent<float> evt) {
            vfxValue = (int)evt.newValue;
            _settingViewModel.SetSFXValue(vfxValue);
        }
        #endregion

        #region DropDown
        private void StyleDropdownItems() {
            //var content = topElement.rootVisualElement.parent.panel.visualTree.Q<VisualElement>(className: "unity-base-dropdown");
            //if (content != null) {
            //    List<VisualElement> list = content.Query<VisualElement>(className: "unity-base-dropdown__item").ToList();
            //    foreach (VisualElement item in list) {
            //        item.RegisterCallback<PointerEnterEvent>(evt => {
            //            item.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            //        });
            //        item.RegisterCallback<PointerLeaveEvent>(evt => {
            //            item.style.backgroundColor = new Color(0f, 0f, 0f, 1f);
            //        });
            //    }
            //}
        }
        private IEnumerator StyleDropdownItemsNextFrame() {
            // 한 프레임 대기
            yield return null;

            // 스타일 적용
            StyleDropdownItems();
        }
        private void ChangeSlot(ChangeEvent<string> evt) {
            int index = _saveSlotField.index;
            _settingViewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
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
    }
}