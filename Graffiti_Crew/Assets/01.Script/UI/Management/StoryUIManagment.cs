using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class StoryUIManagment : UIManagement {
        private StoryViewModel _viewModel;

        private DialogueView _dialougeView;
        private SettingView _settingView;

        protected override void OnEnable() {
            base.OnEnable();
            DialogueEvent.ShowDialougeViewEvent += ShowDialougeView;
            StageEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialogueEvent.ShowDialougeViewEvent -= ShowDialougeView;
            StageEvent.HideViewEvent -= HideView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new StoryViewModel(_model as StoryModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialogueView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);
            _dialougeView.Show();
        }
        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private void EventFunction() {
            if (_settingView != null) {
                Debug.Log("show");
                ShowView(_settingView);
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
    }
}