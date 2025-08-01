using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class TutorialUIManagment : UIManagement {
        private TutorialView _sprayView;
        private DialogueView _dialougeView;
        private MiniDialogueView _miniDialougeView;
        private SettingView _settingView;

        private StoryViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            DialogueEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialogueEvent.ShowMiniDialougeViewEvent += ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent += SetActiveFightView;
            StageEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialogueEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialogueEvent.ShowMiniDialougeViewEvent -= ShowMiniDialougeView;
            StageEvent.SetActiveFightViewEvent -= SetActiveFightView;
            StageEvent.HideViewEvent -= HideView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new StoryViewModel(_model as TutorialModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);
            _dialougeView = new DialogueView(root.Q<VisualElement>("DialougeView"), _viewModel);
            _miniDialougeView = new MiniDialogueView(root.Q<VisualElement>("MiniDialogBoxView"), _viewModel);
            _sprayView = new TutorialView(root.Q<VisualElement>("SprayView"), _viewModel);
        }
        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private void EventFunction() {
            if (_settingView != null) {
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