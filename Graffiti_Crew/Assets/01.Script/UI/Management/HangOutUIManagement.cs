using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine.UIElements;

namespace AH.UI {
    public class HangOutUIManagement : UIManagement {
        private HangOutViewModel _hangoutViewModel;
        private DialogViewModel _dialogueViewModel;

        private DialougeView _dialougeView;
        private MiniDialougeView _miniDialougeView;
        private SettingView _settingView;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent += ShowMiniDialougeView;
            HangOutEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.ShowMiniDialougeViewEvent -= ShowMiniDialougeView;
            HangOutEvent.HideViewEvent -= HideView;
        }

        protected override void Init() {
            base.Init();
            _hangoutViewModel = new HangOutViewModel(_model as HangOutModel);
            _dialogueViewModel = new DialogViewModel(_model as DialogModel);
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialougeView(root.Q<VisualElement>("DialogBoxView"), _dialogueViewModel);
            _miniDialougeView = new MiniDialougeView(root.Q<VisualElement>("MiniDialogBoxView"), _dialogueViewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _hangoutViewModel);
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
    }
}
