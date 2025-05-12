using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class FightView : UIView {
        private FightViewModel _viewModel;

        private VisualElement _fRivalborder;
        private VisualElement _fPlayerborder;
        private VisualElement _aPlayerborder;

        private VisualElement _sprayOutLine;

        private bool _notEnoughSpray = false;

        public FightView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            StageEvent.ChangeSprayValueEvent += UpdateSpray;
            StageEvent.SetViewEvnet += SetView;
        }
        public override void Initialize() {
            base.Initialize();
            _viewModel = viewModel as FightViewModel;
            UpdateSpray();
        }
        public override void Dispose() {
            StageEvent.ChangeSprayValueEvent -= UpdateSpray;
            StageEvent.SetViewEvnet -= SetView;
            base.Dispose();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _fRivalborder = topElement.Q<VisualElement>("fight-rival-border");
            _fPlayerborder = topElement.Q<VisualElement>("fight-player-border");
            _aPlayerborder = topElement.Q<VisualElement>("activity-player-border");
            _sprayOutLine = topElement.Q<VisualElement>("spray-outline");
        }

        public override void Show() {
            base.Show();
            _sprayOutLine.style.unityBackgroundImageTintColor = new StyleColor(Color.white);
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
        }
        private async void NotEnoughSpray() {
            while (_notEnoughSpray) {
                _sprayOutLine.ToggleInClassList("spray-size-up");
                _sprayOutLine.ToggleInClassList("spray-color-change");
                await Task.Delay(300);
            }
        }
        private void SetView(bool fight) {
            if (fight) {
                _fRivalborder.style.display = DisplayStyle.Flex;
                _fPlayerborder.style.display = DisplayStyle.Flex;
                _aPlayerborder.style.display = DisplayStyle.None;
            }
            else {
                _fRivalborder.style.display = DisplayStyle.None;
                _fPlayerborder.style.display = DisplayStyle.None;
                _aPlayerborder.style.display = DisplayStyle.Flex;
            }
        }
    }
}
