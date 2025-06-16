using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class TutorialView : UIView {
        public TutorialView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            var hide = topElement.Q<VisualElement>("top-container");
            hide.AddToClassList("hide");
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
    }
}