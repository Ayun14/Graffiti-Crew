using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ResultView : UIView {
        public Button retryBtn;
        public Button exitBtn;

        public ResultView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            retryBtn = topElement.Q<Button>("retry-btn");
            exitBtn = topElement.Q<Button>("exit-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            retryBtn.RegisterCallback<ClickEvent>(ClickRetryBtn);
            exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            retryBtn.UnregisterCallback<ClickEvent>(ClickRetryBtn);
            exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }

        private void ClickRetryBtn(ClickEvent evt) {
            Debug.Log("아직 연결 안했어용");
        }
        private void ClickExitBtn(ClickEvent evt) {
            SceneManager.LoadScene("ComputerScene");
        }
    }
}