using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        private FightViewModel ViewModel;

        private VisualElement _resultPanel;
        private VisualElement _playerScreen;
        private VisualElement _rivalScreen;

        private Button _retryBtn;
        private Button _exitBtn;

        //private bool _playerWin = false;

        public ResultView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            ViewModel = viewModel as FightViewModel;
        }
        public override void Initialize() {
            base.Initialize();
            StageEvent.ShowVictorScreenEvent += FullScreen;
        }
        public override void Dispose() {
            base.Dispose();
            StageEvent.ShowVictorScreenEvent -= FullScreen;
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _resultPanel = topElement.Q<VisualElement>("result-container");
            _playerScreen = topElement.Q<VisualElement>("player-screen");
            _rivalScreen = topElement.Q<VisualElement>("rival-screen");
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _retryBtn.UnregisterCallback<ClickEvent>(ClickRetryBtn);
            _exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }
        private async void FullScreen(bool result) {
            if (result) {
                _resultPanel.AddToClassList("result-in");
                await Task.Delay(250);
                SetPlayerResultView();
            }
            else {
                var buttonBorder = topElement.Q<VisualElement>("button-border");
                Debug.Log("remove");
                buttonBorder.AddToClassList("hide-button-border");
                SetRivalResultView();
            }
        }
        private void SetPlayerResultView() {
            _exitBtn = _resultPanel.Q<Button>("exit-btn");
            _retryBtn = _resultPanel.Q<Button>("retry-btn");
            _retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        private void SetRivalResultView() {
            var buttonBorder = topElement.Q<VisualElement>("button-border");
            _exitBtn = buttonBorder.Q<Button>("exit-btn");
            _retryBtn = buttonBorder.Q<Button>("retry-btn");
            _retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        private void ClickRetryBtn(ClickEvent evt) {
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("FightScene");
        }
        private void ClickExitBtn(ClickEvent evt) {
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("ComputerScene");
        }
    }
}