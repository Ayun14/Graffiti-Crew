using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        private FightViewModel ViewModel;

        private VisualElement _cResultPanel;
        private VisualElement _lResultPanel;

        private Button[] _nextBtns;

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

            _nextBtns = topElement.Query<Button>("next-btn").ToList().ToArray();
        }
        public override void Show() {
            base.Show();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            for(int i = 0; i < _nextBtns.Length; i++) {
                _nextBtns[i].RegisterCallback<ClickEvent>(ClickNextBtn);
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            for (int i = 0; i < _nextBtns.Length; i++) {
                _nextBtns[i].UnregisterCallback<ClickEvent>(ClickNextBtn);
            }
        }

        private void FullScreen(bool result) {
            if (result) {
                SetStar();
                _cResultPanel.AddToClassList("result-in");
            }
            else {
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
                Debug.LogError("야 박아름 해결해");
            }
            for(int i = 0; i < 3 - currentStageData.star; i++) {
                stars[i].RemoveFromClassList("star");
            }
        }
        private void ClickNextBtn(ClickEvent evt) {
            StageEvent.ClickNectBtnEvent?.Invoke();
        }
    }
}