using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class UISystem : MonoBehaviour {
        private UIDocument _uiDocument;

        private ComputerView _computerView; // 이거 여기서 안쓸 것 같음
        private SettingView _settingView;
        private FightView _fightView;

        [Header("Input")]
        [SerializeField] private InputReaderSO _inputReaderSO;
        private Stack<UIView> _viewStack = new Stack<UIView>();
        private List<UIView> _allViews = new List<UIView>();

        [Header("Models")]
        [SerializeField] private ComputerModel computerModel;
        private ComputerViewModel _computerViewModel;

        private void Awake() {
            //_inputReaderSO.OnCancleEvent += ShowPreviewEvent;
        }
        void OnEnable() {
            _uiDocument = GetComponent<UIDocument>();

            SetupViews();
            RegisterToEvents();

            // Start with the home screen
            ShowView(_fightView);
        }
        
        void OnDisable() {
            //_inputReaderSO.OnCancleEvent -= ShowPreviewEvent;
            UnRegisterToEvents();

            foreach (UIView view in _allViews) {
                view.Dispose();
            }
        }

        private void ShowPreviewEvent() {
            if(_viewStack.Count > 0) {
                var view = _viewStack.Peek();
                if(view != null) {
                    view.Hide();
                    _viewStack.Pop();
                }
            }
        }
        private void ShowView(UIView newView, bool offPreview = false) {
            if (offPreview) { // 이전뷰 끄기
                ShowPreviewEvent();
            }
            if(newView != null) {
                _viewStack.Push(newView);
                var view = _viewStack.Peek();
                if (view != null) {
                    view.Show();
                }
            }
        }

        private void SetupViews() {
            VisualElement root = _uiDocument.rootVisualElement;

            _computerViewModel = new ComputerViewModel(computerModel);

            _computerView = new ComputerView(root.Q<VisualElement>("ComputerView"), _computerViewModel);
            _settingView = new SettingView(root.Q<VisualElement>("TestView"), _computerViewModel);
            _fightView = new FightView(root.Q<VisualElement>("FightView"), _computerViewModel);

            _allViews.Add(_computerView);
            _allViews.Add(_settingView);
            _allViews.Add(_fightView);
        }

        private void RegisterToEvents() { }
        private void UnRegisterToEvents() { }
    }
}
