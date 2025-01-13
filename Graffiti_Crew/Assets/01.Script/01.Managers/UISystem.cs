using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class UISystem : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private UIView _currentView; // �����
        private UIView _previousView; // ������

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
            if (_currentView != null) { // ���� ���� �ִ� view�� ������ ��
                _currentView.Hide();
            }

            _previousView = _currentView;
            _currentView = newView;

            if (_currentView != null) { // ���� ���� ������ �װ� ���ְ� ���� ���� �ִ� view�� ��������� �˷���
                _currentView.Show();
            }
        }

        // �̺�Ʈ ��� �� ����
        private void RegisterToEvents() {

        }
        private void UnRegisterToEvents() {

        }
    }
}