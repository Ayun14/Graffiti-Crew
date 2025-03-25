using AH.UI.Events;
using AH.UI.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StageDescriptionView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualElement _startBtn;
        private List<Button> _selectFriendBtnList;

        private AdmissionTicket[] tickets;
        private VisualTreeAsset _ticketAsset;

        private Button _exitBtn;

        private IDisposable _friend1Btn;

        public StageDescriptionView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _ticketAsset = Resources.Load<VisualTreeAsset>("UI/Stage/Ticket");
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _startBtn = topElement.Q<VisualElement>("start-btn");
            _exitBtn = topElement.Q<Button>("exit-btn");
            SetAdmissionTicket();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _startBtn.RegisterCallback<ClickEvent>(ClickStartGameBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _friend1Btn.Dispose();
            _startBtn.UnregisterCallback<ClickEvent>(ClickStartGameBtn);
            _exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }

        private void ClickExitBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }

        public override void Show() {
            SetAdmissionTicket();
            base.Show();
        }

        private void SetAdmissionTicket() {
            tickets = ComputerViewModel.GetStageDescription().ticket;
            var content = topElement.Q<VisualElement>("ticket-content");
            content.Clear();
            foreach (var data in tickets) {
                var asset = _ticketAsset.Instantiate();
                asset.Q<VisualElement>("spray-img").style.backgroundImage = new StyleBackground(data.ticketType.image);
                asset.Q<Label>("count-txt").text = data.count.ToString();
                content.Add(asset);
            }
        }
        private bool CheckTicket() {
            return ItemSystem.CheckTicket(tickets);
        }
        private void ClickStartGameBtn(ClickEvent evt) {
            if (CheckTicket()) {
                ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
                GameEvents.SaveGameEvent?.Invoke();
                SceneManager.LoadScene("FightScene");
            }
            else {
                // 경고창 or 붉을색으로 못 누르게 변경 해야함
                Debug.Log("입장권이 부족합니다");
            }
        }
    }
}