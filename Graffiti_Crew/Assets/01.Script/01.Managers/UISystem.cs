using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class UISystem : MonoBehaviour {
        private UIDocument _uiDocument;

        private UIView _currentView;
        private UIView _previousView;

        private List<UIView> _allViews = new List<UIView>();

        private ComputerView _computerView;
        private SettingView _settingView;

        [Header("Models")]
        [SerializeField] private ComputerModel computerModel;
        private ComputerViewModel _computerViewModel;

        void OnEnable() {
            _uiDocument = GetComponent<UIDocument>();

            SetupViews();
            RegisterToEvents();

            // Start with the home screen
            ChangeShowView(_computerView);
        }

        void OnDisable() {
            UnRegisterToEvents();

            foreach (UIView view in _allViews) {
                view.Dispose();
            }
        }

        private void SetupViews() {
            VisualElement root = _uiDocument.rootVisualElement;

            _computerViewModel = new ComputerViewModel(computerModel);

            _computerView = new ComputerView(root.Q<VisualElement>("ComputerView"), _computerViewModel);
            _settingView = new SettingView(root.Q<VisualElement>("TestView"), _computerViewModel);

            _allViews.Add(_computerView);
            _allViews.Add(_settingView);

            _settingView.Show();
        }

        private void ChangeShowView(UIView newView) {
            if (_currentView != null) {
                _currentView.Hide();
            }

            _previousView = _currentView;
            _currentView = newView;

            if (_currentView != null) {
                _currentView.Show();
            }
        }

        private void RegisterToEvents() { }

        private void UnRegisterToEvents() { }
    }
}
