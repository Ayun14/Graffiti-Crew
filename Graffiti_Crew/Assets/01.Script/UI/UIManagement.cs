using AH.UI.Events;
using AH.UI.Models;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManagement : MonoBehaviour {
    protected UIDocument _uiDocument;
    protected Stack<UIView> _viewStack = new Stack<UIView>();

    [SerializeField] protected Model _model;
    [SerializeField] protected InputReaderSO _inputReaderSO;

    protected virtual void Awake() {
        _uiDocument = GetComponent<UIDocument>();
    }
    protected virtual void OnEnable() {
        _inputReaderSO.OnCancleEvent += ShowPreviewEvent;
    }
    protected virtual void OnDisable() {
        _inputReaderSO.OnCancleEvent -= ShowPreviewEvent;
    }
    protected void ShowPreviewEvent() {
        if (_viewStack.Count > 0) {
            var view = _viewStack.Peek();
            if (view != null) {
                view.Hide();
                _viewStack.Pop();
            }
        }
        else if (_viewStack.Count == 0) {
            _viewStack.Clear();
            UIEvents.CloseComputerEvnet?.Invoke();
            SceneManager.LoadScene("SY");
        }
    }
    protected void ShowView(UIView newView, bool offPreview = false) {
        if (offPreview) { // ¿Ã¿¸∫‰ ≤Ù±‚
            ShowPreviewEvent();
        }
        if (newView != null) {
            _viewStack.Push(newView);
            var view = _viewStack.Peek();
            if (view != null) {
                view.Show();
            }
        }
    }
    protected void HidwView() {
        var view = _viewStack.Peek();
        if (view != null) {
            view.Hide();
            _viewStack.Pop();
        }
    }
}
