using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class UISystem : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private UIView _currentView; // 현재뷰
        private UIView _previousView; // 이전뷰

        private List<UIView> _allViews = new List<UIView>();

        private UIView _mainView;

        public const string mainViewName = "TestView";

        void OnEnable() {
            _uiDocument = GetComponent<UIDocument>();

            SetupViews();
            RegisterToEvents();

            // Start with the home screen
            ChangeShowView(_mainView);
        }
        void OnDisable() {
            UnRegisterToEvents();

            foreach (UIView view in _allViews) {
                view.Dispose();
            }
        }

        private void SetupViews() {
            VisualElement root = _uiDocument.rootVisualElement;

            //_mainView = new TestView(root.Q<VisualElement>(mainViewName)); // Landing modal screen

            _allViews.Add(_mainView);

            _mainView.Show();
        }
        private void ChangeShowView(UIView newView) {
            if (_currentView != null) { // 지금 보고 있는 view가 있으면 꺼
                _currentView.Hide();
            }

            _previousView = _currentView;
            _currentView = newView;

            if (_currentView != null) { // 지금 볼거 있으면 그거 켜주고 지금 보고 있는 view가 변경됬음을 알려줘
                _currentView.Show();
            }
        }

        // 이벤트 등록 및 해제
        private void RegisterToEvents() {

        }
        private void UnRegisterToEvents() {

        }
    }
}