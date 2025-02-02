using AH.UI.ViewModels;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectFriendView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualTreeAsset _profileAsset;
        private ScrollView _crewScrollView;

        private int _currentBtnIndex = -1;
        public int CurrentBtnIndex { get { return _currentBtnIndex; } set { _currentBtnIndex = value; } }

        public SelectFriendView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _profileAsset = Resources.Load<VisualTreeAsset>("UI/Stage/CrewMemberProfile");
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _crewScrollView = topElement.Q<ScrollView>("crew-scrollView");
            foreach(var data in ComputerViewModel.GetCrew().crew) {
                var asset = _profileAsset.Instantiate();
                asset.Q<Label>("name-txt").text = data.name;
                asset.Q<Label>("ability-txt").text = data.ability;
                asset.Q<VisualElement>("profile-img").style.backgroundImage = new StyleBackground(data.profile);
                _crewScrollView.Add(asset);
            }
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int index = 0;
            foreach (var child in _crewScrollView.Children()) {
                child.RegisterCallback<ClickEvent, int>(SelectMember, index++);
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }

        private void SelectMember(ClickEvent evt, int index) {
            ComputerViewModel.SetFriendImg(CurrentBtnIndex, index);
            CurrentBtnIndex = -1;
            this.Hide();
        }

    }
}
