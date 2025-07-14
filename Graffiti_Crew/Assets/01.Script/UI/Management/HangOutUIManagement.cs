using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class HangOutUIManagement : UIManagement {
        private HangOutViewModel _hangoutViewModel;
        private DialogViewModel _dialogueViewModel;

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
            _hangoutViewModel = new HangOutViewModel(_model as HangOutModel);
            _dialogueViewModel = new DialogViewModel(_model as DialogModel);
            if (!_hangoutViewModel.IsPlayTutorial()) {
                Debug.Log("move stop");
                HangOutEvent.SetPlayerMovementEvent?.Invoke(false);
            }
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialogueView(root.Q<VisualElement>("DialogBoxView"), _dialogueViewModel);
            //_miniDialougeView = new MiniDialogueView(root.Q<VisualElement>("MiniDialogBoxView"), _dialogueViewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _settingViewModel);
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
    }
}
