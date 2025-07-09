using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class FightView : UIView {
        private FightViewModel _viewModel;

        private VisualElement _fightBorder;
        private VisualElement _activitBorder;
        private GameProgressElement _fGameProgress;
        private GameProgressElement _aGameProgress;

        private VisualElement _sprayOutLine;
        private bool _notEnoughSpray = false;

        private StageType stageType = StageType.Battle;

        public FightView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            StageEvent.ChangeSprayValueEvent += UpdateSpray;
            StageEvent.ChangeGameProgressValueEvent += UpdateGameProgress;
            StageEvent.SetProgressEvnet += SetProgress;
        }
        public override void Dispose() {
            StageEvent.ChangeSprayValueEvent -= UpdateSpray;
            StageEvent.ChangeGameProgressValueEvent -= UpdateGameProgress;
            StageEvent.SetProgressEvnet -= SetProgress;
            base.Dispose();
        }
        public override void Initialize() {
            _viewModel = viewModel as FightViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _sprayOutLine = topElement.Q<VisualElement>("spray-outline");

            _fightBorder = topElement.Q<VisualElement>("fight-progress-border");
            _activitBorder = topElement.Q<VisualElement>("activity-progress-border");
            _fGameProgress = _fightBorder.Q<GameProgressElement>("game-progress");
            _aGameProgress = _activitBorder.Q<GameProgressElement>("game-progress");

            if (_viewModel.GetLoadStageSO().stage.Contains("Battle")) {
                stageType = StageType.Battle;
            }
            else {
                stageType = StageType.Activity;
            }
        }
        public override void Show() {
            base.Show();
            Debug.Log("show fight view");
            SetGameProgress();
            _sprayOutLine.style.unityBackgroundImageTintColor = new StyleColor(Color.white);
        }

        private void SetGameProgress() {
            if(stageType == StageType.Battle) {
                _fGameProgress.SetImage(_viewModel.GetJiaImg(), _viewModel.GetRivalImg());
                _fGameProgress.Min = _viewModel.GetGameProgressSO().min;
                _fGameProgress.Max = _viewModel.GetGameProgressSO().max;
            }
            else {
                _aGameProgress.SetImage(_viewModel.GetJiaImg(), _viewModel.GetRivalImg());
                _aGameProgress.Max = _viewModel.GetGameProgressSO().max;
                _aGameProgress.Min = _viewModel.GetGameProgressSO().min;
            }
            UpdateGameProgress();
        }
        private void UpdateGameProgress() {
            if (stageType == StageType.Battle) {
                _fGameProgress.Value = _viewModel.GetGameProgressSO().Value;
            }
            else {
                _aGameProgress.Value = _viewModel.GetGameProgressSO().Value;
            }
        }
        private void UpdateSpray() {
            if (_viewModel.GetSprayData().Value <= Mathf.Epsilon) {
                if (!_notEnoughSpray) { // 이미하고 있는지 확인
                    _notEnoughSpray = true;
                    NotEnoughSpray();
                }
            }
            else {
                _notEnoughSpray = false;
            }

            float gaugeRatio = _viewModel.GetSprayData().Value / _viewModel.GetSprayData().max;

            // change color
            Color healthColor = Color.Lerp(Color.red, Color.white, gaugeRatio);

            // Update health bar color
            if (_sprayOutLine != null) {
                _sprayOutLine.style.unityBackgroundImageTintColor = new StyleColor(healthColor);
            }
        }
        private async void NotEnoughSpray() {
            while (_notEnoughSpray) {
                _sprayOutLine.ToggleInClassList("spray-size-up");
                _sprayOutLine.ToggleInClassList("spray-color-change");
                await Task.Delay(300);
            }
        }
        private void SetProgress(bool fight) {
            if (fight) {
                _fightBorder.style.display = DisplayStyle.Flex;
                _activitBorder.style.display = DisplayStyle.None;
            }
            else {
                _fightBorder.style.display = DisplayStyle.None;
                _activitBorder.style.display = DisplayStyle.Flex;
            }
        }
    }
}
