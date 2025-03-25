using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class StoryUIManagment : UIManagement {
        private DialougeView _dialougeView;

        private RequestViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new RequestViewModel(_model as StoryModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialougeView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _dialougeView.Show();
        }
        private void ShowDialougeView(bool active) {
            if (active) {
                _dialougeView.Show();
            }
            else {
                _dialougeView.Hide();
            }
        }
    }
}