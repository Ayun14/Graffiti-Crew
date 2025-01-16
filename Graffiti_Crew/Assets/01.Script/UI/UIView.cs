using System;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public abstract class UIView : IDisposable {
        protected bool isOverlay; // 부분 투명 여부
        protected bool hideOnAwake = true;
        protected VisualElement topElement; // templeateContainer 말하는거임

        public ViewModel viewModel;
        public VisualElement Root => topElement;
        public bool IsTransparent => isOverlay;
        public bool IsHidden => topElement.style.display == DisplayStyle.None;

        public UIView(VisualElement topContainer, ViewModel viewModel) {
            // null이 아니라면 m_TopElement에 topElement넣어주고 
            this.topElement = topContainer ?? throw new ArgumentNullException(nameof(topContainer));
            this.viewModel = viewModel;

            Initialize();
        }

        public virtual void Initialize() {
            if (hideOnAwake) {
                //Hide();
            }
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        // 세팅
        protected virtual void SetVisualElements() {

        }

        // 콜백 등록 및 해제
        protected virtual void RegisterButtonCallbacks() {

        }
        protected virtual void UnRegisterButtonCallbacks() {

        }

        public virtual void Show() {
            topElement.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide() {
            topElement.style.display = DisplayStyle.None;
        }

        // 이벤트 핸들러를 등록 해제
        public virtual void Dispose() {
            UnRegisterButtonCallbacks();
        }
    }
}

