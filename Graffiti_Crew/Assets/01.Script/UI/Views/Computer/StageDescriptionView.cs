using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StageDescriptionView : UIView {
        private ComputerViewModel ComputerViewModel;

        private Label _stageName;
        private VisualElement _grafftiImg;
        private Label _stageDescription;
        private Button _startBtn;

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
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
    }
}