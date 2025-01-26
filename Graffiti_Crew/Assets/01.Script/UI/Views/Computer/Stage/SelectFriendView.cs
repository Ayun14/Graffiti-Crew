using AH.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectFriendView : UIView {
        private ComputerViewModel ComputerViewModel;

        private ListView _crewListView;
        private Button _closeBtn;

        private int _currentBtnIndex = -1;
        public int CurrentBtnIndex { get { return _currentBtnIndex; } set { _currentBtnIndex = value; } }

        public SelectFriendView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _crewListView = topElement.Q<ListView>("crew-listView");
            _crewListView.itemsSource = ComputerViewModel.GetCrew().crew;
            _closeBtn = topElement.Q<Button>("close-btn");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();

            // 항목을 선택했을 때
            _crewListView.selectionChanged += OnItemSelected;
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _crewListView.selectionChanged -= OnItemSelected;
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
        }

        private void ClickCloseBtn(ClickEvent evt) {
            this.Hide();
        }

        private void OnItemSelected(IEnumerable<object> selectedItems) {
            var selectedItem = selectedItems.FirstOrDefault();

            if (selectedItem != null) {
                int selectedIndex = _crewListView.selectedIndex;
                ComputerViewModel.SetFriendImg(_currentBtnIndex, selectedIndex);
                CurrentBtnIndex = -1;
            }
            //this.Hide();
        }
    }
}
