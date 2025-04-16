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

        private VisualElement _itemCountView;
        private VisualElement _hideScreen;

        public StoreView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _productAsset = Resources.Load<VisualTreeAsset>("UI/Store/ProductProfile");

            ComputerEvent.BuyItemEvent += BuyItem;
            base.Initialize();
        }

        public override void Dispose() {
            ComputerEvent.BuyItemEvent -= BuyItem;
            base.Dispose();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _productScrollView = topElement.Q<ScrollView>("category-scrollView");
            _exitBtn = topElement.Q<Button>("exit-btn");
            _hideScreen = topElement.Q<VisualElement>("check-hide-input");
            ComputerViewModel.ClearSelectProductData();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int scrollviewIndex = 0;
            //foreach (var child in _productScrollView.Children()) {
            //    VisualElement checker = child.Q<VisualElement>("click-checker");
            //    Debug.Log(checker);
            //    checker.RegisterCallback<ClickEvent, int>(SelectProduct, scrollviewIndex++);
            //}
            int categoryBtnIndex = 0;
            foreach(var button in _categoryBtnList) {
                button.RegisterCallback<ClickEvent, int>(ClickCategory, categoryBtnIndex++);
            }
            _hideScreen.RegisterCallback<PointerDownEvent>(HideItemCountView);
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
            _hideScreen.UnregisterCallback<PointerDownEvent>(HideItemCountView);
            _exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }
        public override void Show() {
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[0]);
            base.Show();
        }

        #region Category
        private void ClickExitBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }
        private void ClickCategory(ClickEvent evt, int index) {
            _categoryIndex = index;
            ShowCurrentCategory(ComputerViewModel.GetCategory().categoryList[_categoryIndex]); // 새로운 category 띄우기
            ComputerViewModel.ClearSelectProductData(); // 보이던 상세 데이터 싹 비우고
        }
        private void ShowCurrentCategory(ProductCategorySO category) {
            _productScrollView.Clear();
            _categoryBtnList.Clear();
            foreach (var item in category.products) {
                VisualElement asset = _productAsset.Instantiate();
                asset.Q<Label>("name-txt").text = item.itemName;
                asset.Q<Label>("price-txt").text = item.price.ToString();
                asset.Q<VisualElement>("item-img").style.backgroundImage = new StyleBackground(item.image);
                asset.Q<VisualElement>("buy-btn").RegisterCallback<PointerDownEvent, (ProductSO, VisualElement, int)>(ClickBuyProduct, (item, asset, 1));
                SetPossessionItem(item, asset);

                _productScrollView.Add(asset);
            }
            _categoryBtnList = topElement.Query<Button>(className: "category-btn").ToList();
            RegisterButtonCallbacks();
        }
        #endregion

        #region Item
        private void SetPossessionItem(ProductSO item, VisualElement asset) {
            Label possessionTxt = asset.Q<Label>("possession-txt");
            bool isHave = ComputerViewModel.HaveItem(item);
            string txt = isHave ? "보유중" : "미보유";
            Color color = isHave ? Color.green : Color.red;
            possessionTxt.style.color = new StyleColor(color);
            possessionTxt.text = txt.ToString();
        }
        private void SelectProduct(ClickEvent evt, int index) {
            ComputerViewModel.SetSelectProduct(_categoryIndex, index);
        }
        private void ClickBuyProduct(PointerDownEvent evt, (ProductSO, VisualElement, int) data) {
            if (evt.button == 0) {
                BuyItem(data);
            }
            else if (evt.button == 1) {
                ComputerEvent.SetItemCountViewPosEvent?.Invoke(evt.position, data);
                ComputerEvent.ActiveItemCountViewEvent?.Invoke(true);
            }
        }

        private void BuyItem((ProductSO, VisualElement, int) data) {
            if (data.Item1.BuyItem(data.Item3)) { // 구매할 수 있음(돈 계산 함)
                Debug.Log(data.Item3);
                ItemSystem.AddItem(data.Item1, data.Item3);
                GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Buy);
                SetPossessionItem(data.Item1, data.Item2);
            }
            else {
                Debug.Log("너 돈 업서");
                ComputerEvent.ShowNotEnoughViewEvent?.Invoke();
            }
        }
        #endregion

        private void HideItemCountView(PointerDownEvent evt) {
            ComputerEvent.ActiveItemCountViewEvent?.Invoke(false);
        }
    }
}
