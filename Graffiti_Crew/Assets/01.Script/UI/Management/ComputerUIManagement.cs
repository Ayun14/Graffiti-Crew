using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class ComputerUIManagement : UIManagement {
        private ComputerView _computerView;
        private SelectChapterView _selectChapterView;
        private StoreView _storeView;
        private StageDescriptionView _stageDescriptionView;
        private ItemCountView _itemCountView;
        private NotEnoughView _notEnoughView;

        private ComputerViewModel _viewModel;

        protected override void OnEnable() {
            base.OnEnable();
            ComputerEvent.ShowSelectChapterViewEvent += ShowSelectChapterView;
            ComputerEvent.ShowStageDescriptionViewEvent += ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent += ShowStoreView;
            ComputerEvent.ActiveItemCountViewEvent += ShowItemCountView;
            ComputerEvent.ShowNotEnoughViewEvent += ShowNotEnoughView;

            ComputerEvent.HideViewEvent += HideView;
            PresentationEvents.FadeInOutEvent += FadeInOut;
        }
        protected override void OnDisable() {
            base.OnDisable();
            ComputerEvent.ShowSelectChapterViewEvent -= ShowSelectChapterView;
            ComputerEvent.ShowStageDescriptionViewEvent -= ShowStageDescriptionView;
            ComputerEvent.ShowStoreViewEvent -= ShowStoreView;
            ComputerEvent.ActiveItemCountViewEvent -= ShowItemCountView;
            ComputerEvent.ShowNotEnoughViewEvent -= ShowNotEnoughView;

            ComputerEvent.HideViewEvent -= HideView;
            PresentationEvents.FadeInOutEvent -= FadeInOut;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new ComputerViewModel(_model as ComputerModel);
        }
        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _computerView = new ComputerView(root.Q<VisualElement>("ComputerView"), _viewModel);
            _selectChapterView = new SelectChapterView(root.Q<VisualElement>("SelectChapterView"), _viewModel);
            _storeView = new StoreView(root.Q<VisualElement>("StoreView"), _viewModel);
            _stageDescriptionView = new StageDescriptionView(root.Q<VisualElement>("StageDescriptionView"), _viewModel);
            _itemCountView = new ItemCountView(root.Q<VisualElement>("ItemCountView"), _viewModel);
            _notEnoughView = new NotEnoughView(root.Q<VisualElement>("not-enough-view"), _viewModel);

            Fade();
            _computerView.Show();
        }

        private async void Fade() {
            PresentationEvents.SetFadeEvent?.Invoke(true);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
        }

        protected override void ShowPreviewEvent(AfterExecution evtFunction = null) {
            evtFunction += EventFunction;
            base.ShowPreviewEvent(evtFunction);
        }
        private async void EventFunction() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }

        private void ShowStoreView() {
            ShowView(_storeView);
        }
        private void ShowSelectChapterView() {
            ShowView(_selectChapterView);
        }
        private void ShowStageDescriptionView() {
            ShowView(_stageDescriptionView);
            //_stageDescriptionView.Hide();
        }
        private void ShowItemCountView(bool active) {
            if (active) {
                ShowView(_itemCountView);
            }
            else {
                if (_itemCountView.Root.style.display == DisplayStyle.Flex) {
                    HideView();
                }
            }
        }
        private void ShowNotEnoughView() {
            ShowView(_notEnoughView);
        }
    }
}