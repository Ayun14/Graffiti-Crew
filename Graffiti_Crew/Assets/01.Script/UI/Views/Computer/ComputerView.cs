using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private ComputerViewModel ComputerViewModel;

        // ui
        private Button _storeBtn;
        private Button _stageBtn;


        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;

            base.Initialize();
        }

        
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _storeBtn = topElement.Q<Button>("store-btn");
            _stageBtn = topElement.Q<Button>("stage-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _storeBtn.RegisterCallback<ClickEvent>(CllickStoreBtn);
            _stageBtn.RegisterCallback<ClickEvent>(CllickStageBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }

        private void CllickStoreBtn(ClickEvent evt) {
            ComputerEvent.ShowStoreViewEvent?.Invoke();
        }
        private void CllickStageBtn(ClickEvent evt) {
            ComputerEvent.ShowSelectStageViewEvent?.Invoke();
        }
    }
}