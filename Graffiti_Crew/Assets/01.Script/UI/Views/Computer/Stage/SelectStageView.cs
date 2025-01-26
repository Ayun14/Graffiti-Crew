using AH.UI.CustomElement;
using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private List<StagePointElement> _stagePointList;
        private List<StagePointElement> _requestPointList;

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
            
            _stagePointList = topElement.Query<StagePointElement>("stage-point").ToList();
            _requestPointList = topElement.Query<StagePointElement>("request-point").ToList();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn, (button.chapter, button.stage));
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                button.UnregisterCallback<ClickEvent, (string chapter, string stage) > (ClickStageBtn);
            }
        }

        private void ClickStageBtn(ClickEvent evt, (string chapter, string stage) data) {
            // 본인 텍스트에 적힌 이름을 바탕으로 맵 스폰해주면 될 듯
            ComputerViewModel.SetStageData($"Chapter{data.chapter}", $"Stage{data.stage}");
            _stageDescriptionView.Show();
        }
    }
}
