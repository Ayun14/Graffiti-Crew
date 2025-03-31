using AH.UI.Data;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StoreView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualTreeAsset _productAsset;
        private ScrollView _productScrollView;

        private Button _exitBtn;

        private List<Button> _categoryBtnList = new List<Button>();
        private int _categoryIndex;

        public StoreView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _productAsset = Resources.Load<VisualTreeAsset>("UI/Store/ProductProfile");

            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _productScrollView = topElement.Q<ScrollView>("category-scrollView");
            _exitBtn = topElement.Q<Button>("exit-btn");
            ComputerViewModel.ClearSelectProductData();
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[0]);
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int scrollviewIndex = 0;
            foreach (var child in _productScrollView.Children()) {
                child.RegisterCallback<ClickEvent, int>(SelectProduct, scrollviewIndex++);
            }
            int categoryBtnIndex = 0;
            foreach(var button in _categoryBtnList) {
                button.RegisterCallback<ClickEvent, int>(ClickCategory, categoryBtnIndex++);
            }
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var child in _productScrollView.Children()) {
                child.UnregisterCallback<ClickEvent, int>(SelectProduct);
            }
            foreach (var button in _categoryBtnList) {
                button.UnregisterCallback<ClickEvent, int>(ClickCategory);
            }
            _exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }

        private void ClickExitBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }

        private void ShowCurrentCategory(ProductCategorySO category) {
            _productScrollView.Clear();
            _categoryBtnList.Clear();
            foreach (var data in category.products) {
                var asset = _productAsset.Instantiate();
                asset.Q<Label>("name-txt").text = data.itemName;
                asset.Q<Label>("price-txt").text = data.price.ToString();
                asset.Q<VisualElement>("item-img").style.backgroundImage = new StyleBackground(data.image);
                asset.Q<Button>("buy-btn").RegisterCallback<ClickEvent, ProductSO>(ClickBuyProduct, data);
                _productScrollView.Add(asset);
            }
            _categoryBtnList = topElement.Query<Button>(className: "category-btn").ToList();
            RegisterButtonCallbacks();
        }
        private void SelectProduct(ClickEvent evt, int index) {
            ComputerViewModel.SetSelectProduct(_categoryIndex, index);
        }
        private void ClickCategory(ClickEvent evt, int index) {
            _categoryIndex = index;
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[_categoryIndex]); // 새로운 category 띄우기
            ComputerViewModel.ClearSelectProductData(); // 보이던 상세 데이터 싹 비우고
        }
        private void ClickBuyProduct(ClickEvent evt, ProductSO item) {
            if (item.BuyItem()) { // 구매할 수 있음(돈 계산 함)
                ItemSystem.AddItem(item);
                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Buy);
            }
            else {
                // 여기서 돈이 -인지 아닌지 bool로 받고 그거에 따라서 구매 실패 띄우기
            }
        }
    }
}
