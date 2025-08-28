using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using UnityEngine.UIElements;

public enum DialougeCharacter {
    Text,
    Felling
}
namespace AH.UI.Views {
    public class DialogueView : UIView {
        private DialogViewModel ViewModel;

        private VisualElement _textDialouge;
        private VisualElement _fellingDialouge;
        private VisualElement _currentDialouge;
        private VisualElement _preDialouge;

        private VisualElement[] _arrowImgs;
        private Button _skipBtn;

        public DialogueView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ViewModel = viewModel as DialogViewModel;

            DialogueEvent.SetDialogueEvent += SetCharscter;
            DialogueEvent.EndWritingText += EndWritingText;
        }
        public override void Dispose() {
            DialogueEvent.SetDialogueEvent -= SetCharscter;
            DialogueEvent.EndWritingText -= EndWritingText;

            base.Dispose();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();

            _textDialouge = topElement.Q<VisualElement>("text-dialog");
            _fellingDialouge = topElement.Q<VisualElement>("feeling-dialog");

            _arrowImgs = topElement.Query<VisualElement>("arrow-img").ToList().ToArray();

            _skipBtn = topElement.Q<Button>("skip-btn");

            Hide(_textDialouge);
            Hide(_fellingDialouge);
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

            _skipBtn.RegisterCallback<ClickEvent>(ClickSkipBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();

            _skipBtn.UnregisterCallback<ClickEvent>(ClickSkipBtn);
        }

        private void ClickSkipBtn(ClickEvent evt) {
            DialogueEvent.SkipToStory?.Invoke();
        }

        private void SetCharscter(DialougeCharacter character) {
            _arrowImgs[0].RemoveFromClassList("on-arrow");
            _arrowImgs[1].RemoveFromClassList("on-arrow");

            switch (character) {
                case DialougeCharacter.Text:
                    _preDialouge = _currentDialouge;
                    _currentDialouge = _textDialouge;
                    break;
                case DialougeCharacter.Felling:
                    _preDialouge = _currentDialouge;
                    _currentDialouge = _fellingDialouge;
                    break;
            }
            ShowAndHide(_currentDialouge, _preDialouge);
        }
        private void EndWritingText() {
            _arrowImgs[0].AddToClassList("on-arrow");
            _arrowImgs[1].AddToClassList("on-arrow");
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