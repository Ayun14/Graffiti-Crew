using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private List<Button> _stagePointList;
        private List<Button> _requestPointList;

        private const string _selectStageViewName = "StageDescriptionView";
        private StageDescriptionView _stageDescriptionView;


        public SelectStageView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {

        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _stageDescriptionView = new StageDescriptionView(topElement.Q<VisualElement>(_selectStageViewName), viewModel);
            
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            
            _stagePointList = topElement.Query<Button>("stage-point").ToList();
            _requestPointList = topElement.Query<Button>("request-point").ToList();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                button.RegisterCallback<ClickEvent>(ClickStageBtn);
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                button.UnregisterCallback<ClickEvent>(ClickStageBtn);
            }
        }

        private void ClickStageBtn(ClickEvent evt) {
            // 본인 텍스트에 적힌 이름을 바탕으로 맵 스폰해주면 될 듯
            _stageDescriptionView.Show();
        }
    }
}
