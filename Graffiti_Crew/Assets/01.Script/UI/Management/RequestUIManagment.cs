using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class RequestUIManagment : UIManagement {
        private RequestView _sprayView;
        private DialougeView _dialougeView;
        private DialougeView _miniDialougeView;

        private RequestViewModel _viewModel;

        private VisualElement _fadeView;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent += ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent += SetActiveFightView;
            PresentationEvents.FadeInOutEvent += FadeInOut;
            PresentationEvents.SetFadeEvent += SetFade;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent -= ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent -= SetActiveFightView;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            PresentationEvents.SetFadeEvent -= SetFade;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new RequestViewModel(_model as RequestModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _sprayView = new RequestView(root.Q<VisualElement>("SprayView"), _viewModel);
            _dialougeView = new DialougeView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _miniDialougeView = new DialougeView(root.Q<VisualElement>("MiniDialogBoxView"), _viewModel);

            _fadeView = root.Q<VisualElement>("fade-view");

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
        private void FadeInOut(bool active) {
            if (active) {
                _fadeView.RemoveFromClassList("fade-out");
            }
            else {
                _fadeView.AddToClassList("fade-out");
            }
        }
        private void SetFade(bool startBlack)
        {
            if (startBlack)
            {
                Debug.Log("fade");
                _fadeView.AddToClassList("fade-out");
            }
            else
            {
                _fadeView.RemoveFromClassList("fade-out");
            }
        }
    }
}