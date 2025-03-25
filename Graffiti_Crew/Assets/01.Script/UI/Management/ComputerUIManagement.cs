using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI {
    public class ComputerUIManagement : UIManagement {
        private ComputerView _computerView;
        private SelectStageView _selectStageView;
        private StoreView _storeView;
        private StageDescriptionView _stageDescriptionView;

        private ComputerViewModel _viewModel;

        private VisualElement _fadeView;

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
            ComputerEvent.ShowSelectStageViewEvent += ShowSelectStageView;
            ComputerEvent.ShowStageDescriptionViewEvent += ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent += ShowStoreView;
            ComputerEvent.HideViewEvent += HideView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            ComputerEvent.ShowSelectStageViewEvent -= ShowSelectStageView;
            ComputerEvent.ShowStageDescriptionViewEvent -= ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent -= ShowStoreView;
            ComputerEvent.HideViewEvent -= HideView;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
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
            _stageDescriptionView = new StageDescriptionView(root.Q<VisualElement>("StageDescriptionView"), _viewModel);

            _fadeView = root.Q<VisualElement>("fade-view");

            _computerView.Show();
        }

        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private void EventFunction() {
            UIEvents.CloseComputerEvnet?.Invoke();
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("HangOutScene"); // 이거 변경해야해 코드를 받아서 실행하거나 하는 식으로 
        }

        private void ShowStoreView() {
            ShowView(_storeView);
        }
        private void ShowSelectStageView() {
            ShowView(_selectStageView);
        }
        private void ShowStageDescriptionView() {
            ShowView(_stageDescriptionView);
        }
    }
}