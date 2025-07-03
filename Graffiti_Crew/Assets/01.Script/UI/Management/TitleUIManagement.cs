using AH.Save;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;
using static AH.Save.SaveSystem;

namespace AH.UI {

    public class TitleUIManagement : UIManagement {
        private TitleViewModel _viewModel;

        private SaveSlotView _slotView;
        private SettingView _settingView;

        [SerializeField] private BoolSaveDataSO _checkFirstLoad;
        [SerializeField] private LoadStageSO _loadStageSO;

        private Button _startBtn;
        private Button _settingBtn;
        private Button _exitBtn;

        private VisualElement _fadeView;
        private void Awake() {
            SaveHelperSystem.SetSaveSystem(FindFirstObjectByType<SaveSystem>());
        }

        protected override void Start()
        {
            base.Start();
            GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Title);
        }

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
            StageEvent.HideViewEvent += HideView;
            TitleEvent.StartGameEvent += StartGame;
        }
        protected override void OnDisable() {
            base.OnDisable();
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            StageEvent.HideViewEvent -= HideView;
            TitleEvent.StartGameEvent -= StartGame;
        }

        protected override void Init() {
            base.Init();

            _viewModel = new TitleViewModel(_model as TitleModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);
            _slotView = new SaveSlotView(root.Q<VisualElement>("SaveSlotView"), _viewModel);

            _startBtn = root.Q<Button>("open-slot-btn");
            _settingBtn = root.Q<Button>("setting-save-btn");
            _exitBtn = root.Q<Button>("exit-btn");
            _fadeView = root.Q<VisualElement>("fade-view");
        }
        protected override void Register() {
            base.Register();
            _startBtn.RegisterCallback<ClickEvent>(ShowSlotView);
            _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
            
        }
        private async void StartGame() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await System.Threading.Tasks.Task.Delay(1100);

            _loadStageSO.SetCurrentStage("Chapter1Activity0", StageType.Activity);
            _loadStageSO.chapter = "Chapter1";
            _loadStageSO.stage = "Activity0";

            string sceneName = _checkFirstLoad.data ? "HangOutScene" : "TutorialScene";
            SaveDataEvents.SaveGameEvent?.Invoke(sceneName);
        }
        #region Handle
        private void ShowSlotView(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            ShowView(_slotView);
        }
        private void ClickSettingBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            ShowView(_settingView);
            evt.StopPropagation();
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