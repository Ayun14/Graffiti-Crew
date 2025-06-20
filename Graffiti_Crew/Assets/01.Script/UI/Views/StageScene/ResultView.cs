using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        private FightViewModel ViewModel;

        private VisualElement _cResultPanel;
        private VisualElement _lResultPanel;

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
        private void FullScreen(bool result) {
            if (result) {
                ClearPanel();
                _cResultPanel.AddToClassList("result-in");
            }
            else {
                FailPanel();
                _lResultPanel.AddToClassList("result-in");
            }
        }
        private void SetStar()
        {
            List<VisualElement> stars = topElement.Query<VisualElement>(className : "star").ToList();
            StageSaveDataSO currentStageData = null;
            string path = "";
            string stageName = ViewModel.GetStageName();
            string pattern = @"^(Chapter\d+)(Activity\d+|Battle\d+)$";
            Match match = Regex.Match(stageName, pattern);

            if (match.Success) {
                string chapter = match.Groups[1].Value;  // 예: Chapter123
                string name = match.Groups[2].Value;  // 예: Battle456 또는 Activity2

                path = $"SaveData/{chapter}/{name}";
            }
            currentStageData = Resources.Load<StageSaveDataSO>(path);
            if (currentStageData == null) {
                Debug.LogError(path);
                Debug.LogError("야 박아름 해결해");
            }
            for(int i = 0; i < 3 - currentStageData.star; i++) {
                stars[i].RemoveFromClassList("star");
            }
        }

        private void ClearPanel() {
            SetStar();
            Button homeBtn = _cResultPanel.Q<Button>("clear-btn");
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