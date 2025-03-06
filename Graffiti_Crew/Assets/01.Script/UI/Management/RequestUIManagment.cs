using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class RequestUIManagment : UIManagement {
        private RequestView _sprayView;

        private RequestViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
        }
        protected override void OnDisable() {
            base.OnDisable();
        }

        protected override void Init() {
            base.Init();
            _viewModel = new RequestViewModel(_model as RequestModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _sprayView = new RequestView(root.Q<VisualElement>("SprayView"), _viewModel);

            _sprayView.Show();
        }
    }
}