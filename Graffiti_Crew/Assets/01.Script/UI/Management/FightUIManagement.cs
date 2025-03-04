using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class FightUIManagement : UIManagement {
        private FightView _fightView;
        private DialougeView _dialougeView;
        private ResultView _resultView;

        private FightViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            FightEvent.SetActiveFightViewEvent += SetActiveFightView;
            FightEvent.ShowFightViewEvent += ShowFightView;
            FightEvent.HideFightViewEvent += HideFightView;
            FightEvent.ShowResultViewEvent += ShowResultView;
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent += HideDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            FightEvent.SetActiveFightViewEvent -= SetActiveFightView;
            FightEvent.ShowFightViewEvent -= ShowFightView;
            FightEvent.HideFightViewEvent -= HideFightView;
            FightEvent.ShowResultViewEvent -= ShowResultView;
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent -= HideDialougeView;
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
        private void ShowDialougeView() {
            _dialougeView.Show();
        }
        private void HideDialougeView() {
            _dialougeView.Hide();
        }
        private void ShowFightView() {
            _fightView.Show();
        }
        private void HideFightView() {
            _fightView.Hide();
        }
        #endregion
        protected override void Init() {
            base.Init();
            _viewModel = new FightViewModel(_model as FightModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _fightView = new FightView(root.Q<VisualElement>("FightView"), _viewModel);
            _dialougeView = new DialougeView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _resultView = new ResultView(root.Q<VisualElement>("ResultView"), _viewModel);

            _fightView.Show();
        }
        private void SetActiveFightView(bool active) {
            if (active) {
                _fightView.Show();
            }
            else {
                _fightView.Hide();
            }
        }
    }
}