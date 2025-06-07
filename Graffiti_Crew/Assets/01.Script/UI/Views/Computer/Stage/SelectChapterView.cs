using AH.SaveSystem;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectChapterView : UIView {
        private ComputerViewModel ComputerViewModel;

        private Button _exitMap;

        private List<StagePointElement> _pointList;
        private List<StageSaveDataSO> _saveStageData = new List<StageSaveDataSO>();

        public SelectChapterView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            hideOnAwake = false;
           
            base.Initialize();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _exitMap = topElement.Q<Button>("exit-btn");

            _pointList = topElement.Query<StagePointElement>(className: "stage-point").ToList();
            _saveStageData = ComputerViewModel.GetSaveStageDatas();
            LoadSaveData();

            SetSaveDataToStagePoint();
        }

        private void LoadSaveData() {
            for(int i = 0; i < 3; i++) {
                //_pointList[i].StageType = _saveStageData[i].stageType;
                _pointList[i].StageState = _saveStageData[i].stageState;
            }
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _pointList) {
                if (button.StageState != StageState.Lock) {
                    switch (button.StageType) {
                        case StageType.Battle:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickBattleBtn, (button.chapter, button.stage));
                            break;
                        case StageType.Activity:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickActivityBtn, (button.chapter, button.stage));
                            break;
                        case StageType.Story:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn, (button.chapter, button.stage));
                            break;
                    }
                }
            }
            _exitMap.RegisterCallback<ClickEvent>(CllickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _pointList) {
                if (button.StageState != StageState.Lock) {
                    if (button.StageType == StageType.Battle) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickBattleBtn);
                    }
                    else if (button.StageType == StageType.Story) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn);
                    }
                    else {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickActivityBtn);
                    }
                }
            }
            _exitMap.UnregisterCallback<ClickEvent>(CllickExitBtn);
        }

        private void SetSaveDataToStagePoint() {
            int index = 0;
            int length = Mathf.Min(_saveStageData.Count, _pointList.Count);
            while (index < length) {
                if(_saveStageData[index].stageState == StageState.CanPlay || _saveStageData[index].stageState == StageState.Lock) { // 다음 스테이지 보이기
                    _pointList[index].StageState = StageState.CanPlay;
                    break;
                }
                _pointList[index].StageState = _saveStageData[index].stageState;
                //_pointList[index].starCount = _saveStageData[index].star;
                index++;
            }
        }
        private async void CllickExitBtn(ClickEvent evt) {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }
        private void ClickBackBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }
        #region ClickStages
        private void ClickBattleBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Battle{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStageData(chapter, stage, StageType.Battle);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
        }
        private void ClickStoryBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Story{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStoryData(chapter, stage, StageType.Story);
            SaveDataEvents.SaveGameEvent?.Invoke("StoryScene");
        }
        private void ClickActivityBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Activity{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetActivityData(chapter, stage, StageType.Activity);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
        }
        #endregion
    }
}
