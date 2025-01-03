using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace AH.UI {
    public abstract class UIView : IDisposable {
        protected bool isOverlay; // �κ� ���� ����
        protected bool hideOnAwake = true;
        protected VisualElement topElement; // templeateContainer ���ϴ°���

        public VisualElement Root => topElement;
        public bool IsTransparent => isOverlay;
        public bool IsHidden => topElement.style.display == DisplayStyle.None;

        public UIView(VisualElement topContainer) {
            // null�� �ƴ϶�� m_TopElement�� topElement�־��ְ� 
            this.topElement = topContainer ?? throw new ArgumentNullException(nameof(topContainer));

            Initialize();
        }

        public virtual void Initialize() {
            if (hideOnAwake) {
                //Hide();
            }
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        // ����
        protected virtual void SetVisualElements() {

        }

        // �ݹ� ��� �� ����
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

        // �̺�Ʈ �ڵ鷯�� ��� ����
        public virtual void Dispose() {
            UnRegisterButtonCallbacks();
        }
    }
}

