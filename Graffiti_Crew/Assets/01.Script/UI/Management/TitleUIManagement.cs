using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {

    public class TitleUIManagement : UIManagement {
        private TitleViewModel _viewModel;

        private SettingView _settingView;

        [SerializeField] private BoolSaveDataSO _checkFirstLoad;
        [SerializeField] private LoadStageSO _loadStageSO;

        private Button _startBtn;
        private Button _settingBtn;
        private Button _saveSlotBtn;
        private Button _exitBtn;

        private VisualElement _slotView;
        private List<Button> _slotList;
        private Button _closeBtn;
        private SlotSO[] slots;
        private string slotPath = "UI/Setting/Slots/";

        private VisualElement _fadeView;

        protected override void Start()
        {
            base.Start();

            GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Title);
        }

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
            //_inputReaderSO.OnPressAnyKeyEvent += PressAnyKey;
            StageEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            //_inputReaderSO.OnPressAnyKeyEvent -= PressAnyKey;
            StageEvent.HideViewEvent -= HideView;
        }

        protected override void Init() {
            base.Init();
            slots = Resources.LoadAll<SlotSO>(slotPath);

            _viewModel = new TitleViewModel(_model as TitleModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);
            _slotView = root.Q<VisualElement>("slot-content");

            _startBtn = root.Q<Button>("start-btn");
            _settingBtn = root.Q<Button>("setting-save-btn");
            _saveSlotBtn = root.Q<Button>("saveSlot-btn");
            _exitBtn = root.Q<Button>("exit-btn");
            _fadeView = root.Q<VisualElement>("fade-view");
            _closeBtn = root.Q<Button>("slot-close-btn");

            _slotList = root.Query<Button>(className: "slot-btn").ToList();

            _slotView.style.display = DisplayStyle.None;
        }

        protected override void Register() {
            base.Register();
            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);
            _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
            _saveSlotBtn.RegisterCallback<ClickEvent, bool>(ClickActiveSaveSlotView, true);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
            _closeBtn.RegisterCallback<ClickEvent, bool>(ClickActiveSaveSlotView, false);

            int index = 0;
            foreach(Button btn in _slotList) {
                btn.RegisterCallback<ClickEvent, int>(ChangeSlot, index);
                index++;
            }
        }
        private void ClickActiveSaveSlotView(ClickEvent evt, bool active) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            if (active) {
                _slotView.style.display = DisplayStyle.Flex;
            }
            else {
                _slotView.style.display = DisplayStyle.None;
            }
        }

        private async void Fade() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await System.Threading.Tasks.Task.Delay(1100);

            _loadStageSO.SetCurrentStage("Chapter1Activity0", StageType.Activity);
            _loadStageSO.chapter = "Chapter1";
            _loadStageSO.stage = "Activity0";

            string sceneName = _checkFirstLoad.data ? "HangOutScene" : "TutorialScene";
            SaveDataEvents.SaveGameEvent?.Invoke(sceneName);
        }

        #region DropDown
        private void ChangeSlot(ClickEvent evt, int index) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            _settingViewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
            Fade();
        }
        #endregion
        #region Handle
        private void PressAnyKey(AfterExecution execution) {
            ClickStartBtn(null);
        }
        private void ClickStartBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            Fade();
        }
        private void ClickSettingBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            ShowView(_settingView);
            evt.StopPropagation();
        }
        private void ClickDeleteSaveDataBtn(ClickEvent evt) {
            SaveDataEvents.DeleteSaveDataEvent?.Invoke();
        }
        private void ClickExitBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            Application.Quit();
            evt.StopPropagation();
        }
        #endregion
    }
}