using AH.UI.Events;
using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private ComputerViewModel ComputerViewModel;

        private Button _storeBtn;
        private Button _stageBtn;

        private Button _exitBtn;

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
            _exitBtn = topElement.Q<Button>("exit-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _storeBtn.RegisterCallback<ClickEvent>(CllickStoreBtn);
            _stageBtn.RegisterCallback<ClickEvent>(CllickStageBtn);
            _exitBtn.RegisterCallback<ClickEvent>(CllickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _storeBtn.UnregisterCallback<ClickEvent>(CllickStoreBtn);
            _stageBtn.UnregisterCallback<ClickEvent>(CllickStageBtn);
            _exitBtn.UnregisterCallback<ClickEvent>(CllickExitBtn);
        }

        private void CllickStoreBtn(ClickEvent evt) {
            ComputerEvent.ShowStoreViewEvent?.Invoke();
        }
        private void CllickStageBtn(ClickEvent evt) {
            ComputerEvent.ShowSelectChapterViewEvent?.Invoke();
           // ComputerEvent.ShowSelectStageViewEvent?.Invoke();
        }
        private void CllickExitBtn(ClickEvent evt) {
            //UIEvents.CloseComputerEvnet?.Invoke();
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }
    }
}