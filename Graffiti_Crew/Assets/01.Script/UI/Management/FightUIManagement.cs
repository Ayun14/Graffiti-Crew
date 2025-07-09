using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class FightUIManagement : UIManagement {
        private FightView _fightView;
        private DialogueView _dialougeView;
        private ResultView _resultView;
        private SettingView _settingView;

        private FightAnimationView _fightAnimationView;

        private FightViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            StageEvent.SetActiveFightViewEvent += SetActiveFightView;
            StageEvent.ShowResultViewEvent += ShowResultView;
            DialogueEvent.ShowDialougeViewEvent += ShowDialougeView;
            StageEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            StageEvent.SetActiveFightViewEvent -= SetActiveFightView;
            StageEvent.ShowResultViewEvent -= ShowResultView;
            DialogueEvent.ShowDialougeViewEvent -= ShowDialougeView;
            StageEvent.HideViewEvent -= HideView;
        }
        protected override void Init() {
            base.Init();
            _viewModel = new FightViewModel(_model as FightModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _fightView = new FightView(root.Q<VisualElement>("FightView"), _viewModel);
            _resultView = new ResultView(root.Q<VisualElement>("ResultView"), _viewModel);
            _fightAnimationView = new FightAnimationView(root.Q<VisualElement>("Animation"), _viewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);

            _fightAnimationView.Show();
        }
        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private void EventFunction() {
            if (_settingView != null) {
                ShowView(_settingView);
            }
        }
        #region Handle
        private void ShowResultView(bool active, bool winner) {
            if (active) {
                StageEvent.ShowVictorScreenEvent?.Invoke(winner);
                _resultView.Show();
            }
            else {
                _resultView.Hide();
            }
        }
        private void ShowDialougeView(bool active) {
            if (active) {
                _dialougeView.Show();
            }
            else {
                _dialougeView.Hide();
            }
        }
        private void SetActiveFightView(bool active) {
            if (active) {

                _fightView.Show();
            }
            else {
                _fightView.Hide();
            }
        }
        #endregion
    }
}