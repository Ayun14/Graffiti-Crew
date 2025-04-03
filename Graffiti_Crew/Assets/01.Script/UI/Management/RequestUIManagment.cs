using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine.UIElements;

namespace AH.UI {
    public class RequestUIManagment : UIManagement {
        private RequestView _sprayView;
        private DialogueView _dialougeView;
        private DialogueView _miniDialougeView;

        private RequestViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            DialogueEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialogueEvent.ShowMiniDialougeViewEvent += ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent += SetActiveFightView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialogueEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialogueEvent.ShowMiniDialougeViewEvent -= ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent -= SetActiveFightView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new RequestViewModel(_model as RequestModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _sprayView = new RequestView(root.Q<VisualElement>("SprayView"), _viewModel);
            _dialougeView = new DialogueView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _miniDialougeView = new DialogueView(root.Q<VisualElement>("MiniDialogBoxView"), _viewModel);

            _sprayView.Show();
        }
        private void ShowDialougeView(bool active) {
            if (active) {
                _dialougeView.Show();
            }
            else {
                _dialougeView.Hide();
            }
        }
        private void ShowMiniDialougeView(bool active) {
            if (active) {
                _miniDialougeView.Show();
            }
            else {
                _miniDialougeView.Hide();
            }
        }
        private void SetActiveFightView(bool active) {
            if (active) {
                _sprayView.Show();
            }
            else {
                _sprayView.Hide();
            }
        }
    }
}