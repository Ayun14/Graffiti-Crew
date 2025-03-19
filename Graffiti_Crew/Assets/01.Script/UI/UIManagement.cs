using AH.UI.Events;
using AH.UI.Models;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine;
using System;

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
        protected Stack<ViewData> _viewStack = new Stack<ViewData>();

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
        }
        protected virtual void OnDisable() {
            _inputReaderSO.OnCancleEvent -= ShowPreviewEvent;
            Unregister();
        }
        protected virtual void Init() {

        }
        protected virtual void SetupViews() {

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
        protected virtual void ShowView(UIView newView, bool offPreview = false, bool hide = false) { // hide : esc·Î ¾È²¨Áü
            if (offPreview) { // ÀÌÀüºä ²ô±â
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
    }
}