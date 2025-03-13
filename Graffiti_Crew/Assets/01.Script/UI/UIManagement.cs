using AH.UI.Events;
using AH.UI.Models;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

        protected void ShowPreviewEvent() {
            if (_viewStack.Count > 0) {
                var viewData = _viewStack.Peek();
                if (viewData != null && !viewData.Hide) {
                    viewData.View.Hide();
                    _viewStack.Pop();
                }
            }
            else if (_viewStack.Count == 0) {
                _viewStack.Clear();
                UIEvents.CloseComputerEvnet?.Invoke();
                SceneManager.LoadScene("HangOutScene"); // 이거 변경해야해 코드를 받아서 실행하거나 하는 식으로 
            }
        }
        protected void ShowView(UIView newView, bool offPreview = false, bool hide = false) { // hide : esc로 안꺼짐
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
        protected void HidwView() {
            var viewData = _viewStack.Peek();
            if (viewData != null && !viewData.Hide) {
                viewData.View.Hide();
                _viewStack.Pop();
            }
        }
    }
}