using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class DialougeView : UIView {
        private VisualElement _leftContent;
        private VisualElement _rightContent;
        public DialougeView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
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
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }

        private void ChangeOpponent() {
            _leftContent.ClassListContains("hide-dialoge");
            _rightContent.ClassListContains("hide-dialoge");
        }
    }
}