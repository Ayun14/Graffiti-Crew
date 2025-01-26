using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StoreView : UIView {
        private ComputerViewModel ComputerViewModel;

        private ListView _categoryListView;

        public StoreView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;

            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _categoryListView = topElement.Q<ListView>("category-listView");
            _categoryListView.itemsSource = ComputerViewModel.GetCategory().productsCategory;
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
    }
}
