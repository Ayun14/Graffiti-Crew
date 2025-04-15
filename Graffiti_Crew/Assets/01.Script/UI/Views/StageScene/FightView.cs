using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class FightView : UIView {
        private FightViewModel _viewModel;

        private ProgressBar _rivalProgress;
        private ProgressBar _playerProgress;
        private ProgressBar _sprayProgress;
        private VisualElement _sprayOutLine;

        private bool _notEnoughSpray = false;

        public FightView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            StageEvent.ChangeSprayValueEvent += UpdateSpray;
        }

        public override void Initialize() {
            base.Initialize();
            _viewModel = viewModel as FightViewModel;
            UpdateSpray();
        }
        public override void Dispose() {
            StageEvent.ChangeSprayValueEvent -= UpdateSpray;
            base.Dispose();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _rivalProgress = topElement.Q<ProgressBar>("rival-progress");
            _playerProgress = topElement.Q<ProgressBar>("player-progress");
            _sprayProgress = topElement.Q<ProgressBar>("spray-total-amount-progress");
            _sprayOutLine = topElement.Q<VisualElement>("spray-outline");
        }
        private void UpdateSpray() {
            if (_viewModel.GetSprayData().value <= 0) {
                if (!_notEnoughSpray) { // 이미하고 있는지 확인
                    _notEnoughSpray = true;
                    NotEnoughSpray();
                }
            }
            else {
                _notEnoughSpray = false;
            }

            float gaugeRatio = _viewModel.GetSprayData().value / _viewModel.GetSprayData().max;

            // change color
            Color healthColor = Color.Lerp(Color.red, Color.white, gaugeRatio);

            // Update health bar color
            if (_sprayOutLine != null) {
                _sprayOutLine.style.unityBackgroundImageTintColor = new StyleColor(healthColor);
            }

            // Update health bar value
            //m_HealthBar.value = gaugeRatio * 100f;
            // Update the status label based on percentage health
            //m_StatusLabel.text = gaugeRatio switch {
            //    >= 0 and < 1.0f / 3.0f => "Danger",
            //    >= 1.0f / 3.0f and < 2.0f / 3.0f => "Neutral",
            //    _ => "Good"
            //};
            // Change the color of the status label to interpolated color
            //m_StatusLabel.style.color = new StyleColor(healthColor);
            // Update value label
            //m_ValueLabel.text = m_HealthModelAsset.CurrentHealth.ToString();
        }
        private async void NotEnoughSpray() {
            while (_notEnoughSpray) {
                _sprayOutLine.ToggleInClassList("spray-size-up");
                _sprayOutLine.ToggleInClassList("spray-color-change");
                await Task.Delay(300);
            }
        }
    }
}
