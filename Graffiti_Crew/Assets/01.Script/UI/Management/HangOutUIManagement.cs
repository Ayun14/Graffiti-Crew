using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class HangOutUIManagement : UIManagement {
        private HangOutViewModel _viewModel;

        private DialougeView _dialougeView;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent += HideDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent -= HideDialougeView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new HangOutViewModel(_model as HangOutModel);
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialougeView(root.Q<VisualElement>("DialogBoxView"), _viewModel);
            Debug.Log(_dialougeView);
        }

        private void ShowDialougeView() {
            _dialougeView.Show();
        }
        private void HideDialougeView() {
            _dialougeView.Hide();
        }
    }
}
