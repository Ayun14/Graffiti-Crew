using AH.UI.ViewModels;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualTreeAsset _stagePointAsset;
        private List<Transform> _chapter1Ratio;

        public SelectStageView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
            ComputerViewModel = viewModel as ComputerViewModel;

            _stagePointAsset = ComputerViewModel.GetStagePointAsset();
            _chapter1Ratio = ComputerViewModel.GetChapter1Ratio();

            SpawnPoint(_chapter1Ratio);
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
        private void SpawnPoint(List<Transform> list) {
            var container = topElement.Q<VisualElement>("background-img");
            foreach (Transform point in list) {
                var asset = _stagePointAsset.Instantiate();
                container.Add(asset);
                asset.style.left = point.position.x - 100;
                asset.style.top = -(point.position.y) - 100;
            }
        }
    }
}
