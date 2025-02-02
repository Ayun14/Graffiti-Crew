using AH.UI.Data;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StoreView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualTreeAsset _productAsset;
        private ScrollView _productScrollView;

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
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[0]);
        }

        private void ShowCurrentCategory(ProductCategorySO category) {
            _productScrollView.Clear();
            _categoryBtnList.Clear();
            foreach (var data in category.products) {
                var asset = _productAsset.Instantiate();
                asset.Q<Label>("name-txt").text = data.itemName;
                asset.Q<Label>("price-txt").text = data.price.ToString();
                asset.Q<VisualElement>("item-img").style.backgroundImage = new StyleBackground(data.image);
                _productScrollView.Add(asset);
            }
            _categoryBtnList = topElement.Query<Button>(className: "category-btn").ToList();
            RegisterButtonCallbacks();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int scrollviewIndex = 0;
            foreach (var child in _productScrollView.Children()) {
                child.RegisterCallback<ClickEvent, int>(SelectMember, scrollviewIndex++);
            }
            int categoryBtnIndex = 0;
            foreach(var button in _categoryBtnList) {
                button.RegisterCallback<ClickEvent, int>(ClickCategory, categoryBtnIndex++);
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var child in _productScrollView.Children()) {
                child.UnregisterCallback<ClickEvent, int>(SelectMember);
            }
            foreach (var button in _categoryBtnList) {
                button.UnregisterCallback<ClickEvent, int>(ClickCategory);
            }
        }

        private void SelectMember(ClickEvent evt, int index) {
            ComputerViewModel.SetSelectProduct(_categoryIndex, index);
        }
        private void ClickCategory(ClickEvent evt, int index) {
            _categoryIndex = index;
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[_categoryIndex]); // 새로운 category 띄우기
            ComputerViewModel.ClearSelectProductData(); // 보이던 상세 데이터 싹 비우고
        }
    }
}
