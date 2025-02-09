using AH.UI.Events;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using UnityEngine.UIElements;

namespace AH.UI {
    public class FightUIManagement : UIManagement {
        private FightView _fightView;
        private DialougeView _dialougeView;

        private FightViewModel _viewModel;

        protected override void Awake() {
            base.Awake();
            Init();
            SetupViews();
        }
        protected override void OnEnable() {
            base.OnEnable();
            FightEvent.SetActiveFightViewEvent += SetActiveFightView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            FightEvent.SetActiveFightViewEvent -= SetActiveFightView;
        }

        private void Init() {
            _uiDocument = GetComponent<UIDocument>();
            _viewModel = new FightViewModel(_model as FightModel);
        }
        private void SetupViews() {
            VisualElement root = _uiDocument.rootVisualElement;

            _fightView = new FightView(root.Q<VisualElement>("FightView"), _viewModel);
            _dialougeView = new DialougeView(root.Q<VisualElement>("DialougeView"), _viewModel);

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