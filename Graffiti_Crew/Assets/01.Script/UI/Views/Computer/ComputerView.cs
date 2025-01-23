using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private List<UIView> _computerViews = new List<UIView>();

        private SelectStageView _selectStageView;
        private StoreView _storeView;

        private const string selectStageViewName = "StoreView";
        private const string storeViewName = "SelectStageView";

        private UIView _currentView;
        private UIView _previousView;

        private ComputerViewModel ComputerViewModel;

        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
            ComputerViewModel = viewModel as ComputerViewModel;

            SetupViews();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
        private void SetupViews() {
            _selectStageView = new SelectStageView(topElement.Q<VisualElement>(storeViewName), ComputerViewModel);
            _storeView = new StoreView(topElement.Q<VisualElement>(selectStageViewName), ComputerViewModel);

            _computerViews.Add(_selectStageView);
            _computerViews.Add(_storeView);

            _selectStageView.Show();
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
    }
}