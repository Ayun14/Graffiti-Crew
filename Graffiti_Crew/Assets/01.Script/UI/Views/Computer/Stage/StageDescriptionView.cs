using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StageDescriptionView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualElement _startBtn;

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
            var content = topElement.Q<VisualElement>("ticket-content");
            content.Clear();
            tickets = ComputerViewModel.GetStageDescription().ticket;

            foreach (var data in tickets) {
                var asset = _ticketAsset.Instantiate();
                if (data.ticketItem) {
                    asset.Q<VisualElement>("spray-img").style.backgroundImage = new StyleBackground(data.ticketItem.image);
                    asset.Q<Label>("count-txt").text = data.count.ToString();
                    content.Add(asset);
                }
            }
        }
        private bool CheckTicket() {
            if (tickets == null) { // 티겟이 없는 스테이지임
                return true;
            }
            return ItemSystem.CheckTicket(tickets);
        }
        private void ClickStartGameBtn(ClickEvent evt) {
            if (CheckTicket()) {
                Debug.Log(ComputerViewModel.GetCurrentStageName());
                Debug.Log(ComputerViewModel.GetCurrentStageName().Contains("Stage"));
                if (ComputerViewModel.GetCurrentStageName().Contains("Stage")) {
                    SaveDataEvents.SaveGameEvent?.Invoke("FightScene");
                }
                else {
                    SaveDataEvents.SaveGameEvent?.Invoke("RequestScene");
                }
            }
            else {
                // 경고창 or 붉을색으로 못 누르게 변경 해야함
                Debug.Log("입장권이 부족합니다");
            }
        }
    }
}