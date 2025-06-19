using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {

    public class TitleUIManagement : UIManagement {
        private TitleViewModel _viewModel;

        private SettingView _settingView;

        [SerializeField] private BoolSaveDataSO _checkFirstLoad;

        private Button _startBtn;
        private Button _settingBtn;
        private Button _exitBtn;
        
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
            _viewModel = new TitleViewModel(_model as TitleModel);
        }
        protected override void SetupViews()
        {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);

            _startBtn = root.Q<Button>("start-btn");
            _settingBtn = root.Q<Button>("setting-save-btn");
            _exitBtn = root.Q<Button>("exit-btn");
            _fadeView = root.Q<VisualElement>("fade-view");

            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);
            _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }

        private async void Fade() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            string sceneName = _checkFirstLoad.data ? "HangOutScene" : "TutorialScene";
            SaveDataEvents.SaveGameEvent?.Invoke(sceneName);
        }
        #region Handle
        private void PressAnyKey(AfterExecution execution) {
            ClickStartBtn(null);
        }
        private void ClickStartBtn(ClickEvent evt) {
            Fade();
        }
        private void ClickSettingBtn(ClickEvent evt) {
            ShowView(_settingView);
            evt.StopPropagation();
        }
        private void ClickDeleteSaveDataBtn(ClickEvent evt) {
            SaveDataEvents.DeleteSaveDataEvent?.Invoke();
        }
        private void ClickExitBtn(ClickEvent evt) {
            Application.Quit();
            evt.StopPropagation();
        }
        #endregion
    }
}