using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class ComputerView : UIView {
        private static ComputerView Instance;
        private ComputerViewModel ComputerViewModel;

        private SelectStageView _selectStageView;
        private StoreView _storeView;

        private const string _selectStageViewName = "StoreView";
        private const string _storeViewName = "SelectStageView";

        private InputReaderSO _inputReaderSO;
        private Stack<UIView> _viewStack = new Stack<UIView>();
        private List<UIView> _computerViews = new List<UIView>();

        // ui
        private Button _storeBtn;
        private Button _stageBtn;


        public ComputerView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            Instance = this;
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _inputReaderSO = ComputerViewModel.GetInputReader();

            base.Initialize();
            SetupViews();
        }
        private void ShowPreviewEvent() {
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
        public static void ShowView(UIView newView, bool offPreview = false) {
            if (offPreview) { // ¿Ã¿¸∫‰ ≤Ù±‚
                Instance.ShowPreviewEvent();
            }
            if (newView != null) {
                Instance._viewStack.Push(newView);
                var view = Instance._viewStack.Peek();
                if (view != null) {
                    view.Show();
                }
            }
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _storeBtn = topElement.Q<Button>("store-btn");
            _stageBtn = topElement.Q<Button>("stage-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _storeBtn.RegisterCallback<ClickEvent>(CllickStoreBtn);
            _stageBtn.RegisterCallback<ClickEvent>(CllickStageBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }

        private void CllickStoreBtn(ClickEvent evt) {
            ShowView(_storeView);
        }
        private void CllickStageBtn(ClickEvent evt) {
            ShowView(_selectStageView);
        }
        private void SetupViews() {
            _selectStageView = new SelectStageView(topElement.Q<VisualElement>(_storeViewName), ComputerViewModel);
            _storeView = new StoreView(topElement.Q<VisualElement>(_selectStageViewName), ComputerViewModel);

            _computerViews.Add(_selectStageView);
            _computerViews.Add(_storeView);
        }

        public override void Show() {
            _inputReaderSO.OnCancleEvent += ShowPreviewEvent;
            base.Show();
        }
        public override void Hide() {
            _inputReaderSO.OnCancleEvent -= ShowPreviewEvent;
            base.Hide();
        }
    }
}