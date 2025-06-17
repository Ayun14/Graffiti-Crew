using AH.SaveSystem;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class StageDescriptionView : UIView {
        private ComputerViewModel ComputerViewModel;

        private VisualElement _stageDescription;

        private List<VisualElement> _coinList;
        private VisualElement _startBtn;
        private Button _closeBtn;

        public StageDescriptionView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            hideOnAwake = false;
            ComputerViewModel = viewModel as ComputerViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _coinList = topElement.Query<VisualElement>(className: "coin").ToList();
            _stageDescription = topElement.Q<VisualElement>("stage-description");
            _startBtn = topElement.Q<VisualElement>("start-btn");
            _closeBtn = topElement.Q<Button>("exit-btn");
        }
        public override void Show() {
            base.Show();
            SetCoin();
            _stageDescription.AddToClassList("stage-description-in");
        }
        public async override void Hide() {
            _stageDescription.RemoveFromClassList("stage-description-in");
            await Task.Delay(350);
            base.Hide();
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _startBtn.RegisterCallback<ClickEvent>(ClickStartGameBtn);
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _startBtn.UnregisterCallback<ClickEvent>(ClickStartGameBtn);
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseBtn);
        }

        private void SetCoin() {
            string path = $"StageData/{ComputerViewModel.GetLoadStageSO().GetLoadStageName()}";
            StageDataSO stageData = Resources.Load<StageDataSO>(path);
            string newPath = $"SaveData/{stageData.nextChapter}/{stageData.nextStage}";
            StageSaveDataSO saveData = Resources.Load<StageSaveDataSO>(newPath);
            for(int i = 0; i < 3 - saveData.star; i++) {
                _coinList[i].RemoveFromClassList("coin");
            }
        }
        private void ClickCloseBtn(ClickEvent evt) {
            //Debug.Log("exit");
            //ComputerEvent.HideViewEvent?.Invoke();
            ComputerEvent.CloseDescriptionEvent?.Invoke();
        }
        private void ClickStartGameBtn(ClickEvent evt) {
            StageType stageType = ComputerViewModel.GetLoadStageSO().GetCurrentStageType();
            switch (stageType) {
                case StageType.Battle:
                    SaveDataEvents.SaveGameEvent?.Invoke("FightScene");
                    break;
                case StageType.Activity:
                    SaveDataEvents.SaveGameEvent?.Invoke("ActivityScene");
                    break;
                case StageType.Story:
                    SaveDataEvents.SaveGameEvent?.Invoke("StoryScene");
                    break;
                case StageType.None:
                    break;
            }
        }
    }
}