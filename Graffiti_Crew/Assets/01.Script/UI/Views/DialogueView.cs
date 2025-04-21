using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
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

        private VisualElement _otherDialouge;
        private VisualElement _jiaDialouge;
        private VisualElement _fellingDialouge;
        private VisualElement _currentDialouge;
        private VisualElement _preDialouge;

        private VisualElement _profile;

        private List<Button> _skipBtnList;


        public DialogueView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            base.Initialize();
            ViewModel = viewModel as DialogViewModel;
            DialogueEvent.SetDialogueEvent += SetCharscter;
            DialogueEvent.ChangeCharacterEvent += ChangeCharacter;
        }

        public override void Dispose() {
            DialogueEvent.SetDialogueEvent -= SetCharscter;
            DialogueEvent.ChangeCharacterEvent -= ChangeCharacter;
            base.Dispose();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _otherDialouge = topElement.Q<VisualElement>("left-dialouge");
            _jiaDialouge = topElement.Q<VisualElement>("right-dialouge");
            _fellingDialouge = topElement.Q<VisualElement>("felling-dialouge");
            _profile = topElement.Q<VisualElement>("profile");

            _skipBtnList = topElement.Query<Button>("skip-btn").ToList();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach(Button btn in _skipBtnList) {
                btn.RegisterCallback<ClickEvent>(ClickSkipBtn);
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (Button btn in _skipBtnList) {
                btn.UnregisterCallback<ClickEvent>(ClickSkipBtn);
            }
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
            Show(_currentDialouge, _preDialouge);
        }
        private void ChangeCharacter() {
            VisualElement emptyView = _currentDialouge;
            _currentDialouge = _preDialouge;
            _preDialouge = emptyView;

            Show(_currentDialouge, _preDialouge);
        }
        public virtual void Show(VisualElement showView, VisualElement hideView) {
            showView.style.display = DisplayStyle.Flex;

            hideView.style.display = DisplayStyle.None;
        }

        private void ClickSkipBtn(ClickEvent evt) {
            DialogueEvent.DialogueSkipEvent?.Invoke();
        }
    }
}