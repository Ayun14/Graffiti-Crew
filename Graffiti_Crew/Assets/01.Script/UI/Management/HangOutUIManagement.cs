using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using UnityEngine.UIElements;

namespace AH.UI {
    public class HangOutUIManagement : UIManagement {
        private HangOutViewModel _viewModel;

        private DialougeView _dialougeView;
        private DialougeView _miniDialougeView;
        private SettingView _settingView;

        private VisualElement _fadeView;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent += ShowMiniDialougeView;
            PresentationEvents.FadeInOutEvent += FadeInOut;
            HangOutEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent -= ShowMiniDialougeView;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            HangOutEvent.HideViewEvent -= HideView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new HangOutViewModel(_model as HangOutModel);
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialougeView(root.Q<VisualElement>("DialogBoxView"), _viewModel);
            _miniDialougeView = new DialougeView(root.Q<VisualElement>("MiniDialogBoxView"), _viewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _viewModel);

            _fadeView = root.Q<VisualElement>("fade-view");
        }
        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }

        private void EventFunction() {
            if (_settingView != null) {
                if (_settingView.Root.style.display == DisplayStyle.Flex){
                    HideView();
                }
                else {
                    ShowView(_settingView);
                }
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
        private void ShowMiniDialougeView(bool active) {
            if (active) {
                _miniDialougeView.Show();
            }
            else {
                _miniDialougeView.Hide();
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
    }
}
