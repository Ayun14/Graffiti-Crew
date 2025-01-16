using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private ComputerViewModel ComputerViewModel;
        private VisualTreeAsset _stagePointAsset;
        private List<Transform> _chapter1Ratio;

        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
            ComputerViewModel = viewModel as ComputerViewModel;

            _stagePointAsset = ComputerViewModel.GetStagePointAsset();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
    }
}