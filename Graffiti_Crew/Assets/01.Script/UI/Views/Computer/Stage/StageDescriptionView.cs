using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StageDescriptionView : UIView {
        private ComputerViewModel ComputerViewModel;

        private const string _selectFriendViewName = "select-friend";

        private Label _stageName;
        private Label _stageDescription;
        private VisualElement _grafftiImg;
        private Button _startBtn;
        private List<Button> _selectFriendBtnList;

        private IDisposable _friend1Btn;

        public StageDescriptionView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;

            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _stageName = topElement.Q<Label>("stage-name-txt");
            _stageDescription = topElement.Q<Label>("stage-description-txt");
            _grafftiImg = topElement.Q<VisualElement>("graffti-img");
            _startBtn = topElement.Q<Button>("start-btn");
            _selectFriendBtnList = topElement.Query<Button>(className : "select-friend-btn").ToList();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int index = 0;
            foreach (var btn in _selectFriendBtnList) {
                btn.RegisterCallback<ClickEvent, int>(ClickSelectFirend, index++);
            }
            _startBtn.RegisterCallback<ClickEvent>(ClickStartGameBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var btn in _selectFriendBtnList) {
                btn.UnregisterCallback<ClickEvent, int>(ClickSelectFirend);
            }
            _friend1Btn.Dispose();
            _startBtn.UnregisterCallback<ClickEvent>(ClickStartGameBtn);
        }

        private void ClickStartGameBtn(ClickEvent evt) {
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
            SceneManager.LoadScene("FightScene");
        }

        private void ClickSelectFirend(ClickEvent evt, int index) {
            ComputerViewModel.currentBtnIndex = index;
            ComputerEvent.ShowSelectFriendViewEvent?.Invoke();
        }
    }
}