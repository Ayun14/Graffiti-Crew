using AH.UI.Events;
using AH.UI.ViewModels;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class DialougeView : UIView {
        private DialogViewModel ViewModel;

        private VisualElement _leftContent;
        private VisualElement _rightContent;

        private VisualElement _profile;

        public DialougeView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ViewModel = viewModel as DialogViewModel;
            DialougeEvent.ChangeOpponentEvent += ChangeOpponent;
        }

        public override void Dispose() {
            base.Dispose();
            DialougeEvent.ChangeOpponentEvent -= ChangeOpponent;
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _leftContent = topElement.Q<VisualElement>("left-bottom-content");
            _rightContent = topElement.Q<VisualElement>("right-bottom-content");
            _profile = topElement.Q<VisualElement>("profile");

            //_leftContent.ClassListContains("hide-dialoge");
            //_rightContent.ClassListContains("hide-dialoge");
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
        private async void ChangeOpponent(Sprite profile) {
            _profile.ToggleInClassList("fade-profile");
            await Task.Delay(350);
        }
    }
}