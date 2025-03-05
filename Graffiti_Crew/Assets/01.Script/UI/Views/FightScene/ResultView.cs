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
            //FightEvent.GameResultEvent += GrowBigWinner;
            //FightEvent.VictorFullScreenEvent += FullScreen;
            FightEvent.ShowVictorScreenEvent += FullScreen;
        }
        public override void Dispose() {
            base.Dispose();
            //FightEvent.GameResultEvent -= GrowBigWinner;
            //FightEvent.VictorFullScreenEvent -= FullScreen;
            FightEvent.ShowVictorScreenEvent -= FullScreen;
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

        /*private void GrowBigWinner(bool result) {
            if (result) {
                GameWin();
            }
            else {
                GameLose();
            }
        }
        private void GameWin() {
            _playerWin = true;
            _playerScreen.AddToClassList("win");
            _rivalScreen.AddToClassList("lose");
        }
        private void GameLose() {
            _playerWin = false;
            _rivalScreen.AddToClassList("win");
            _playerScreen.AddToClassList("lose");
        }*/

        private async void FullScreen(bool result) {
            await Task.Delay(2000);
            if (result) {
                //_playerScreen.AddToClassList("fullscreen");
                //_rivalScreen.AddToClassList("hidescreen");
                //await Task.Delay(600);
                _resultPanel.AddToClassList("result-in");
                //await Task.Delay(250);
                SetPlayerResultView();
            }
            else {
                //_rivalScreen.AddToClassList("fullscreen");
                //_playerScreen.AddToClassList("hidescreen");
                //await Task.Delay(600);
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
            buttonBorder.RemoveFromClassList("hide-button-border");
        }
        private void ClickRetryBtn(ClickEvent evt) {
            SceneManager.LoadScene("FightScene");
        }
        private void ClickExitBtn(ClickEvent evt) {
            SceneManager.LoadScene("ComputerScene");
        }
    }
}