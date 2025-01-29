using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private List<UIView> _computerViews = new List<UIView>();

        private SelectStageView _selectStageView;
        private StoreView _storeView;

        private const string _selectStageViewName = "StoreView";
        private const string _storeViewName = "SelectStageView";

        private InputReaderSO _inputReaderSO;
        private Stack<UIView> _viewStack;

        private ComputerViewModel ComputerViewModel;

        // 지울거
        private TextField _nameField;
        private Button _btn;
        private Label _displayLabel;

        private Button _storeBtn;
        private Button _stageBtn;

        // IDisposable로 구독 관리
        private IDisposable _displayMessageSubscription;

        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            _inputReaderSO.OnCancleEvent += CancelEvent;
            ComputerViewModel = viewModel as ComputerViewModel;

            base.Initialize();
            SetupViews();
        }
        private void CancelEvent() {
            if (_viewStack.Count > 1) {
                var view = _viewStack.Peek();
                view.Show();
                _viewStack.Pop();
            }
            else if (_viewStack.Count == 1) {
                UIEvents.CloseComputerEvnet?.Invoke();
                SceneManager.LoadScene("SY");
            }
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _storeBtn = topElement.Q<Button>("store-btn");
            _stageBtn = topElement.Q<Button>("stage-btn");

            // 지울거
            //_nameField = topElement.Q<TextField>("name-TextField");
            //_btn = topElement.Q<Button>("btn");
            //_displayLabel = topElement.Q<Label>("display-label");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _storeBtn.RegisterCallback<ClickEvent>(CllickStoreBtn);
            _stageBtn.RegisterCallback<ClickEvent>(CllickStageBtn);

            // 지울거
            // ReactiveProperty 구독
            //_displayMessageSubscription = ComputerViewModel.FriendImg
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
            _inputReaderSO.OnCancleEvent -= CancelEvent;
            _displayMessageSubscription?.Dispose();

        }

        private void CllickStoreBtn(ClickEvent evt) {
            _storeView.Show();
        }
        private void CllickStageBtn(ClickEvent evt) {
            _selectStageView.Show();
        }
        private void SetupViews() {
            _selectStageView = new SelectStageView(topElement.Q<VisualElement>(_storeViewName), ComputerViewModel);
            _storeView = new StoreView(topElement.Q<VisualElement>(_selectStageViewName), ComputerViewModel);

            _computerViews.Add(_selectStageView);
            _computerViews.Add(_storeView);
        }
    }
}