using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public enum DialougeCharacter {
    Jia,
    Other,
    Felling
}
namespace AH.UI.Views {
    public class DialogueView : UIView {
        private DialogViewModel ViewModel;

        private VisualElement _jiaDialouge;
        private VisualElement _otherDialouge;
        private VisualElement _fellingDialouge;
        private VisualElement _currentDialouge;
        private VisualElement _preDialouge;


        public DialogueView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ViewModel = viewModel as DialogViewModel;
            DialogueEvent.SetDialogueEvent += SetCharscter;
        }
        public override void Dispose() {
            DialogueEvent.SetDialogueEvent -= SetCharscter;
            base.Dispose();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();

            _jiaDialouge = topElement.Q<VisualElement>("right-dialouge");
            _otherDialouge = topElement.Q<VisualElement>("left-dialouge");
            _fellingDialouge = topElement.Q<VisualElement>("felling-dialouge");
            Hide(_otherDialouge);
            Hide(_jiaDialouge);
            Hide(_fellingDialouge);
        }

        private void SetCharscter(DialougeCharacter character) {
            switch (character) {
                case DialougeCharacter.Jia:
                    _preDialouge = _currentDialouge;
                    _currentDialouge = _jiaDialouge;
                    break;
                case DialougeCharacter.Other:
                    _preDialouge = _currentDialouge;
                    _currentDialouge = _otherDialouge;
                    break;
                case DialougeCharacter.Felling:
                    _preDialouge = _currentDialouge;
                    _currentDialouge = _fellingDialouge;
                    break;
            }
            ShowAndHide(_currentDialouge, _preDialouge);
        }
        public virtual void ShowAndHide(VisualElement showView, VisualElement hideView) {
            if (hideView != null)
            {
                hideView.style.display = DisplayStyle.None;
            }
            if (showView != null)
            {
                showView.style.display = DisplayStyle.Flex;
            }
        }
        private void Hide(VisualElement hideView) {
            if (hideView != null) {
                hideView.style.display = DisplayStyle.None;
            }
        }
    }
}