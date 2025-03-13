using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine.UIElements;

namespace AH.UI {
    public class ComputerUIManagement : UIManagement {
        private ComputerView _computerView;
        private SelectStageView _selectStageView;
        private StoreView _storeView;
        private SelectFriendView _selectFriendView;
        private StageDescriptionView _stageDescriptionView;

        private ComputerViewModel _viewModel;

        private VisualElement _fadeView;

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOut += FadeInOut;
            ComputerEvent.ShowSelectFriendViewEvent += ShowSelectFriendView;
            ComputerEvent.ShowSelectStageViewEvent += ShowSelectStageView;
            ComputerEvent.ShowStageDescriptionViewEvent += ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent += ShowStoreView;
            ComputerEvent.HideViewEvent += HidwView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            ComputerEvent.ShowSelectFriendViewEvent -= ShowSelectFriendView;
            ComputerEvent.ShowSelectStageViewEvent -= ShowSelectStageView;
            ComputerEvent.ShowStageDescriptionViewEvent -= ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent -= ShowStoreView;
            ComputerEvent.HideViewEvent -= HidwView;
            PresentationEvents.FadeInOut -= FadeInOut;
        }

        protected override void Init() {
            base.Init();
            _uiDocument = GetComponent<UIDocument>();
            _viewModel = new ComputerViewModel(_model as ComputerModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _computerView = new ComputerView(root.Q<VisualElement>("ComputerView"), _viewModel);
            _selectStageView = new SelectStageView(root.Q<VisualElement>("SelectStageView"), _viewModel);
            _storeView = new StoreView(root.Q<VisualElement>("StoreView"), _viewModel);
            _selectFriendView = new SelectFriendView(root.Q<VisualElement>("select-friend"), _viewModel);
            _stageDescriptionView = new StageDescriptionView(root.Q<VisualElement>("StageDescriptionView"), _viewModel);

            _fadeView = root.Q<VisualElement>("fade-view");

            _computerView.Show();
        }
        
        private void ShowStoreView() {
            ShowView(_storeView);
        }
        private void ShowSelectStageView() {
            ShowView(_selectStageView);
        }
        private void ShowSelectFriendView() {
            ShowView(_selectFriendView);
        }
        private void ShowStageDescriptionView() {
            ShowView(_stageDescriptionView);
        }
        private void FadeInOut(bool active) {
            if (active) {
                _fadeView.RemoveFromClassList("fade-out");
            }
            else {
                _fadeView.AddToClassList("fade-out");
            }
        }
    }
}