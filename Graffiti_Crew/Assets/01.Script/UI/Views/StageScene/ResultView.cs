using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        private FightViewModel ViewModel;

        private VisualElement _cResultPanel;
        private VisualElement _lResultPanel;

        private Button c_retryBtn;
        private Button c_nextBtn;
        private Button c_quitBtn;

        private Button l_retryBtn;
        private Button l_quitBtn;

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
            _cResultPanel = topElement.Q<VisualElement>("clear-result-container");
            _lResultPanel = topElement.Q<VisualElement>("fail-result-container");
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            c_retryBtn.UnregisterCallback<ClickEvent>(ClickRetryBtn);
            c_quitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }
        private void FullScreen(bool result) {
            if (result) {
                SetStar();
                _cResultPanel.AddToClassList("result-in");
                SetPlayerResultView();
            }
            else {
                _lResultPanel.AddToClassList("result-in");
                SetRivalResultView();
            }
        }

        private void SetStar()
        {
            List<VisualElement> stars = topElement.Query<VisualElement>("star-border").ToList();
            StageSaveDataSO currentStageData = null;
            string stageName = ViewModel.GetStageName();
            StageType stageType = ViewModel.GetStageType();

            switch (stageType) {
                case StageType.Stage:
                    currentStageData = Resources.Load<StageSaveDataSO>($"SaveData/Stage/{stageName}");
                    break;
                case StageType.Request:
                    currentStageData = Resources.Load<StageSaveDataSO>($"SaveData/Request/{stageName}");
                    break;
                case StageType.Story:
                    currentStageData = Resources.Load<StageSaveDataSO>($"SaveData/Story/{stageName}");
                    break;
            }
            for(int i = 0; i < currentStageData.star; i++) {
                stars[0].RemoveFromClassList("star");
            }
        }

        private void SetPlayerResultView() {
            c_nextBtn = _cResultPanel.Q<Button>("next-btn");
            c_retryBtn = _cResultPanel.Q<Button>("retry-btn");
            c_quitBtn = _cResultPanel.Q<Button>("quit-btn");

            c_retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
            c_nextBtn.RegisterCallback<ClickEvent>(ClickNextBtn);
            c_quitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        private void SetRivalResultView() {
            l_retryBtn = _lResultPanel.Q<Button>("retry-btn");
            l_quitBtn = _lResultPanel.Q<Button>("quit-btn");

            l_retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
            l_quitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        private void ClickRetryBtn(ClickEvent evt) {
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("FightScene");
        }
        private void ClickNextBtn(ClickEvent evt) {
            Debug.Log("¿¬°á ¾ÈµÊ");
            //GameEvents.SaveGameEvent?.Invoke();
            //SceneManager.LoadScene("FightScene");
        }
        private void ClickExitBtn(ClickEvent evt) {
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("ComputerScene");
        }
    }
}