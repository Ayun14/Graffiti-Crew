using AH.Save;
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

        private List<VisualElement> _coinShadowList;
        private List<VisualElement> _coinList;
        private VisualElement _startBtn;
        private Button _closeBtn;

        private StyleBackground baseBackgroundImg;

        public StageDescriptionView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
            hideOnAwake = false;
        }

        public override void Initialize() {
            hideOnAwake = false;
            ComputerViewModel = viewModel as ComputerViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _coinList = topElement.Query<VisualElement>(className: "coin").ToList();
            _coinShadowList = topElement.Query<VisualElement>("shadow-coin-box").ToList();
            _stageDescription = topElement.Q<VisualElement>("stage-description");
            _startBtn = topElement.Q<VisualElement>("start-btn");
            _closeBtn = topElement.Q<Button>("exit-btn");
            baseBackgroundImg = _stageDescription.style.backgroundImage;
            Hide();
        }
        public override void Show() {
            SetCoin();
            _stageDescription.AddToClassList("stage-description-in");
       
            topElement.pickingMode = PickingMode.Position;
        }
        public async override void Hide() {
            topElement.pickingMode = PickingMode.Ignore;

            _stageDescription.RemoveFromClassList("stage-description-in");
            await Task.Delay(350);
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
            ResetView();

            string dataPath = ComputerViewModel.GetLoadStageSO().GetLoadStageName();
            StageSaveDataSO saveData = null;
            if (dataPath.Contains("Story")) {
                string path = $"StageData/{dataPath}";
                string newPath = "";

                StageDataSO stageData = Resources.Load<StageDataSO>(path);
                if (stageData.nextChapter == "" || stageData.nextStage == "") {
                    newPath = $"SaveData/{dataPath}";

                    foreach (VisualElement element in _coinShadowList) {
                        element.style.display = DisplayStyle.None;
                    }
                }
                else {
                    newPath = $"SaveData/{stageData.nextChapter}/{stageData.nextStage}";
                }
                saveData = Resources.Load<StageSaveDataSO>(newPath);
            }
            else {
                string newPath = $"SaveData/{dataPath}";
                saveData = Resources.Load<StageSaveDataSO>(newPath); ;
            }

            for (int i = 0; i < _coinList.Count; i++) { // ÃÊ±âÈ­
                _coinList[i].AddToClassList("coin");
            }
            for (int i = 0; i < 3 - saveData.star; i++) {
                _coinList[i].RemoveFromClassList("coin");
            }
        }

        private void ResetView() {
            foreach (VisualElement element in _coinShadowList) {
                element.style.display = DisplayStyle.Flex;
            }
        }

        private void ClickCloseBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            ComputerEvent.HideViewEvent?.Invoke();
        }
        private void ClickStartGameBtn(ClickEvent evt) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

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