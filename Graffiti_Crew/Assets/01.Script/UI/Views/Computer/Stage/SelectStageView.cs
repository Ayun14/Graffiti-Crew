using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private List<StagePointElement> _stagePointList;
        private List<StagePointElement> _requestPointList;

        public SelectStageView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            
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
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Stage{data.stage}";

            ComputerViewModel.SetStageData(chapter, stage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
        }
    }
}
