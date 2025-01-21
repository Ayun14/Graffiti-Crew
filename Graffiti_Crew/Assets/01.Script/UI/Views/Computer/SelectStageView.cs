using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualTreeAsset _stagePointAsset;
        private List<Transform> _chapter1Ratio;

        private List<Button> _stagePointList;

        private const string _selectStageViewName = "StageDescriptionView";
        private StageDescriptionView _stageDescriptionView;

        public SelectStageView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {

        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;

            _stagePointAsset = ComputerViewModel.GetStagePointAsset();
            _chapter1Ratio = ComputerViewModel.GetChapter1Ratio();

            _stageDescriptionView = new StageDescriptionView(topElement.Q<VisualElement>(_selectStageViewName), viewModel);
            _stageDescriptionView.Hide();
            
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            
            SpawnPoint(_chapter1Ratio);

            _stagePointList = topElement.Query<Button>("stage-point").ToList();
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

        private void SpawnPoint(List<Transform> list) {
            var container = topElement.Q<VisualElement>("background-img");
            foreach (Transform point in list) {
                var asset = _stagePointAsset.Instantiate();
                asset.style.left = point.position.x - 100;
                asset.style.top = -(point.position.y) - 100;
                container.Add(asset);
            }
        }
    }
}
