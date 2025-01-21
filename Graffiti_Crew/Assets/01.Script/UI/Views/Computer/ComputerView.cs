using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private List<UIView> _computerViews = new List<UIView>();

        private SelectStageView _selectStageView;
        private StoreView _storeView;

        private const string _selectStageViewName = "StoreView";
        private const string _storeViewName = "SelectStageView";

        private UIView _currentView;
        private UIView _previousView;

        private ComputerViewModel ComputerViewModel;

        private TextField _nameField;
        private Button _btn;
        private Label _displayLabel;

        // IDisposable로 구독 관리
        private IDisposable _displayMessageSubscription;

        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;

            base.Initialize();
            SetupViews();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();

            //_nameField = topElement.Q<TextField>("name-TextField");
            //_btn = topElement.Q<Button>("btn");
            //_displayLabel = topElement.Q<Label>("display-label");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

            // ReactiveProperty 구독
            //_displayMessageSubscription = ComputerViewModel.DisplayMessage
            //    .AsObservable() // AsObservable()를 사용하여 명시적으로 Observable로 변환
            //    .Subscribe(message => {
            //        _displayLabel.text = message;
            //    });

            // 버튼 클릭 이벤트 등록
            //_btn.RegisterCallback<ClickEvent>(ClickBtn);
        }

        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();

            // 구독 해제
            _displayMessageSubscription?.Dispose();
        }

        private void ClickBtn(ClickEvent evt) {
            string inputName = _nameField.text;

            // Command 패턴으로 ViewModel에 요청 전달
            ComputerViewModel.SetUserNameCommand.Execute(inputName);
        }

        private void SetupViews() {
            _selectStageView = new SelectStageView(topElement.Q<VisualElement>(_storeViewName), ComputerViewModel);
            _storeView = new StoreView(topElement.Q<VisualElement>(_selectStageViewName), ComputerViewModel);

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
