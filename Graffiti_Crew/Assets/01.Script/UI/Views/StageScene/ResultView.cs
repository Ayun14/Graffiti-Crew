using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        private FightViewModel ViewModel;

        private VisualElement _cResultPanel;
        private VisualElement _lResultPanel;

        private int _currentStageStarCount = 0;

        public ResultView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            ViewModel = viewModel as FightViewModel;
        }
        public override void Initialize() {
            base.Initialize();
            StageEvent.ShowVictorScreenEvent += FullScreen;
            GameEvents.SendCurrentStarCountEvent += SetCurrentStar;
            Debug.Log("init");
        }
        public override void Dispose() {
            base.Dispose();
            StageEvent.ShowVictorScreenEvent -= FullScreen;
            GameEvents.SendCurrentStarCountEvent -= SetCurrentStar;
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _cResultPanel = topElement.Q<VisualElement>("clear-result-container");
            _lResultPanel = topElement.Q<VisualElement>("fail-result-container");
            Debug.Log("set");
        }
        private void FullScreen(bool result) {
            if (result) {
                ClearPanel();
                Debug.Log("clear");
                _cResultPanel.AddToClassList("result-in");
            }
            else {
                FailPanel();
                _lResultPanel.AddToClassList("result-in");
            }
        }
        private void SetCurrentStar(int count) {
            _currentStageStarCount = count;
        }
        private void SetStar()
        {
            List<VisualElement> stars = topElement.Query<VisualElement>(className : "star").ToList();

            for(int i = 0; i < 3 - _currentStageStarCount; i++) {
                Debug.Log(i);
                stars[i].style.unityBackgroundImageTintColor = new Color(1f, 1f, 1f, 0f);
            }
        }

        private void ClearPanel() {
            SetStar();
            Button homeBtn = _cResultPanel.Q<Button>("home-btn");
            Button nextBtn = _cResultPanel.Q<Button>("next-btn");

            homeBtn.RegisterCallback<ClickEvent>(ClickHomeBtn);
            nextBtn.RegisterCallback<ClickEvent>(ClickNextBtn);
        }
        private void FailPanel() {
            Button retryBtn = _cResultPanel.Q<Button>("retry-btn");
            Button homeBtn = _cResultPanel.Q<Button>("clear-btn");

            homeBtn.RegisterCallback<ClickEvent>(ClickHomeBtn);
            retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
        }

        private void ClickNextBtn(ClickEvent evt) {
            StageEvent.ClickNectBtnEvent?.Invoke();
        }
        private void ClickHomeBtn(ClickEvent evt) {
            SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
        }
        private void ClickRetryBtn(ClickEvent evt) {
            Debug.Log("이거 연결해야해");
            //SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
        }
    }
}