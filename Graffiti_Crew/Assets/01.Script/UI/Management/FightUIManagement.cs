using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine.UIElements;

namespace AH.UI {
    public class FightUIManagement : UIManagement {
        private FightView _fightView;
        private DialogueView _dialougeView;
        private ResultView _resultView;

        private FightAnimation _fightStartAnimation;

        private FightViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            StageEvent.SetActiveFightViewEvent += SetActiveFightView;
            StageEvent.ShowResultViewEvent += ShowResultView;
            DialogueEvent.ShowDialougeViewEvent += ShowDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            StageEvent.SetActiveFightViewEvent -= SetActiveFightView;
            StageEvent.ShowResultViewEvent -= ShowResultView;
            DialogueEvent.ShowDialougeViewEvent -= ShowDialougeView;
        }
        protected override void Init() {
            base.Init();
            _viewModel = new FightViewModel(_model as FightModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _fightView = new FightView(root.Q<VisualElement>("FightView"), _viewModel);
            _dialougeView = new DialogueView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _resultView = new ResultView(root.Q<VisualElement>("ResultView"), _viewModel);
            _fightStartAnimation = new FightAnimation(root.Q<VisualElement>("StartAnimation"), _viewModel);

            _fightStartAnimation.Show();
        }

        #region Handle
        private void ShowResultView(bool active) {
            if (active) {
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