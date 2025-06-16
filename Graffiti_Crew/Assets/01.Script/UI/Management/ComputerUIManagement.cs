using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class ComputerUIManagement : UIManagement {
        private ComputerView _computerView;
        private SelectChapterView _selectChapterView;
        private StoreView _storeView;
        private StageDescriptionView _stageDescriptionView;
        private SettingView _settingView;

        private ItemCountView _itemCountView;
        private NotEnoughView _notEnoughView;

        private ComputerViewModel _viewModel;

        private Button _soundBtn;
        private Button _exitBtn;
        private Label _timeLabel;

        protected override void OnEnable() {
            base.OnEnable();
            ComputerEvent.ShowSelectChapterViewEvent += ShowSelectChapterView;
            ComputerEvent.ShowStageDescriptionViewEvent += ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent += ShowStoreView;
            ComputerEvent.ActiveItemCountViewEvent += ShowItemCountView;
            ComputerEvent.ShowNotEnoughViewEvent += ShowNotEnoughView;

            StageEvent.HideViewEvent += HideView;
            PresentationEvents.FadeInOutEvent += FadeInOut;
        }
        protected override void OnDisable() {
            base.OnDisable();
            ComputerEvent.ShowSelectChapterViewEvent -= ShowSelectChapterView;
            ComputerEvent.ShowStageDescriptionViewEvent -= ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent -= ShowStoreView;
            ComputerEvent.ActiveItemCountViewEvent -= ShowItemCountView;
            ComputerEvent.ShowNotEnoughViewEvent -= ShowNotEnoughView;

            StageEvent.HideViewEvent -= HideView;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new ComputerViewModel(_model as ComputerModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _computerView = new ComputerView(root.Q<VisualElement>("ComputerView"), _viewModel);
            _selectChapterView = new SelectChapterView(root.Q<VisualElement>("SelectChapterView"), _viewModel);
            _storeView = new StoreView(root.Q<VisualElement>("StoreView"), _viewModel);
            _stageDescriptionView = new StageDescriptionView(root.Q<VisualElement>("StageDescriptionView"), _viewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _viewModel);

            _itemCountView = new ItemCountView(root.Q<VisualElement>("ItemCountView"), _viewModel);
            _notEnoughView = new NotEnoughView(root.Q<VisualElement>("not-enough-view"), _viewModel);
            
            _soundBtn = root.Q<Button>("sound-btn");
            _exitBtn = root.Q<Button>("power-btn");
            _timeLabel = root.Q<Label>("time-txt");

            UpdateTime();
            InvokeRepeating(nameof(UpdateTime), 60f, 60f); // 60초마다 갱신

            Fade();
            _computerView.Show();
        }
        protected override void Register() {
            base.Register();
            _soundBtn.RegisterCallback<ClickEvent>(CllickSoundBtn);
            _exitBtn.RegisterCallback<ClickEvent>(CllickExitBtn);
        }
        protected override void Unregister() {
            base.Unregister();
            _soundBtn.UnregisterCallback<ClickEvent>(CllickSoundBtn);
            _exitBtn.UnregisterCallback<ClickEvent>(CllickExitBtn);
        }

        void UpdateTime() {
            DateTime time = DateTime.Now;
            int hour = time.Hour;
            int minute = time.Minute;

            string period;
            int displayHour;

            if (hour == 0) {
                period = "AM";
                displayHour = 12;
            }
            else if (hour < 12) {
                period = "AM";
                displayHour = hour;
            }
            else if (hour == 12) {
                period = "PM";
                displayHour = 12;
            }
            else {
                period = "PM";
                displayHour = hour - 12;
            }

            string text = $"{period} {displayHour:D2}:{minute:D2}";
            _timeLabel.text = text;
        }
        private async void Fade() {
            PresentationEvents.SetFadeEvent?.Invoke(true);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
        }
        private void CllickSoundBtn(ClickEvent evt) {
            ShowView(_settingView);
        }
        private async void CllickExitBtn(ClickEvent evt) {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }
        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private async void EventFunction() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }

        #region ShowView
        private void ShowStoreView() {
            ShowView(_storeView);
        }
        private void ShowSelectChapterView() {
            ShowView(_selectChapterView);
        }
        private void ShowStageDescriptionView() {
            ShowView(_stageDescriptionView);
            //_stageDescriptionView.Hide();
        }
        private void ShowItemCountView(bool active) {
            if (active) {
                ShowView(_itemCountView);
            }
            else {
                if (_itemCountView.Root.style.display == DisplayStyle.Flex) {
                    HideView();
                }
            }
        }
        private void ShowNotEnoughView() {
            ShowView(_notEnoughView);
        }
        #endregion
    }
}