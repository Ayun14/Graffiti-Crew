using AH.UI.Models;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

namespace AH.UI {
    public class ViewData {
        public UIView View { get; }
        public bool Hide { get; }

        public ViewData(UIView view, bool hide) {
            View = view;
            Hide = hide;
        }
    }

    public delegate void AfterExecution();

    public class UIManagement : MonoBehaviour {
        protected UIDocument _uiDocument;
        public UIDocument UIDocument => _uiDocument;
        protected Stack<ViewData> _viewStack = new Stack<ViewData>();


        private VisualElement _fadeView;

        [SerializeField] protected Model _model;
        [SerializeField] protected InputReaderSO _inputReaderSO;

        protected virtual void Start() {
            _uiDocument = GetComponent<UIDocument>();
            Init();
            SetupViews();
            Register();
        }

        #region Base
        protected virtual void OnEnable() {
            _inputReaderSO.OnCancleEvent += ShowPreviewEvent;

            PresentationEvents.SetFadeEvent += SetFade;
            PresentationEvents.FadeInOutEvent += FadeInOut;
        }
        protected virtual void OnDisable() {
            _inputReaderSO.OnCancleEvent -= ShowPreviewEvent;

            PresentationEvents.SetFadeEvent -= SetFade;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            Unregister();
        }
        protected virtual void Init() {
            _uiDocument = GetComponent<UIDocument>();
        }
        protected virtual void SetupViews() {
            _fadeView = _uiDocument.rootVisualElement.Q<VisualElement>("fade-view");
        }
        protected virtual void Register() {

        }
        protected virtual void Unregister() {

        } 
        #endregion

        protected virtual void ShowPreviewEvent(AfterExecution evtFunction = null) {
            if (_viewStack.Count > 0) {
                var viewData = _viewStack.Peek();
                if (viewData != null && !viewData.Hide) {
                    viewData.View.Hide();
                    _viewStack.Pop();
                }
            }
            else if (_viewStack.Count == 0) {
                _viewStack.Clear();

                if (evtFunction != null) {
                    evtFunction();
                }
                
            }
        }
        protected virtual void ShowView(UIView newView, bool offPreview = false, bool hide = false) { // hide : esc로 안꺼짐
            if (offPreview) { // 이전뷰 끄기
                ShowPreviewEvent();
            }
            if (newView != null) {
                _viewStack.Push(new ViewData(newView, hide));
                var viewData = _viewStack.Peek();
                if (viewData != null) {
                    viewData.View.Show();
                }
            }
        }
        protected virtual void HideView() {
            var viewData = _viewStack.Peek();
            if (viewData != null && !viewData.Hide) {
                viewData.View.Hide();
                _viewStack.Pop();
            }
        }

        protected virtual void FadeInOut(bool active) { // 여기서 함수 받고 실행 할 수 있도록 // 밝아지는게 이상함
            _fadeView.RemoveFromClassList("fade-set");
            if (active) {
                _fadeView.RemoveFromClassList("fade-out");
            }
            else {
                _fadeView.AddToClassList("fade-out");
            }
        }
        protected virtual void SetFade(bool startBlack) {
            _fadeView.AddToClassList("fade-set"); // 없으면 추가 있으면 삭제
            if (startBlack) {
                _fadeView.AddToClassList("fade-out");
            }
            else {
                _fadeView.RemoveFromClassList("fade-out");
            }
        }
    }
}