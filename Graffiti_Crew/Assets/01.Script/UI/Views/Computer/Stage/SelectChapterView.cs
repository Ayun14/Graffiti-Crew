using AH.SaveSystem;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
           
            base.Initialize();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _exitMap = topElement.Q<Button>("exit-btn");

            _pointList = topElement.Query<StagePointElement>(className: "stage-point").ToList();
            LoadSaveData();

            SetSaveDataToStagePoint();
        }

        private void LoadSaveData() {
            for (int i = 1; i <= 4; i++) {
                string saveDataPath = $"SaveData/Chapter{i}/";
                List<StageSaveDataSO> list = Resources.LoadAll<StageSaveDataSO>(saveDataPath).ToList();
                _saveStageData.AddRange(list);
            }
            for(int i = 0; i < _saveStageData.Count; i++) {
                _pointList[i].type = _saveStageData[i].stageType;
                _pointList[i].state = _saveStageData[i].stageState;
            }
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _pointList) {
                if (button.state != StageState.Lock) {
                    switch (button.type) {
                        case StageType.Stage:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn, (button.chapter, button.stage));
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
            _exitMap.RegisterCallback<ClickEvent>(ClickBackBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _pointList) {
                if (button.state != StageState.Lock) {
                    if (button.type == StageType.Stage) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn);
                    }
                    else if (button.type == StageType.Story) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn);
                    }
                    else {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickActivityBtn);
                    }
                }
            }
            _exitMap.UnregisterCallback<ClickEvent>(ClickBackBtn);
        }

        private void SetSaveDataToStagePoint() {
            int index = 0;
            int length = Mathf.Min(_saveStageData.Count, _pointList.Count);
            while (index < length) {
                if(_saveStageData[index].stageState == StageState.CanPlay) {
                    break;
                }
                if (_saveStageData[index].stageState == StageState.Lock) {// 다음 스테이지 보이도록
                    _pointList[index].state = StageState.CanPlay; 
                    break;
                }
                _pointList[index].state = _saveStageData[index].stageState;
                //_pointList[index].starCount = _saveStageData[index].star;
                index++;
            }
        }

        private void ClickBackBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }
        #region ClickStages
        private void ClickStageBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Stage{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStageData(chapter, stage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
        }
        private void ClickStoryBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Story{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStoryData(chapter, stage);
            SaveDataEvents.SaveGameEvent?.Invoke("StoryScene");
        }
        private void ClickActivityBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Activity{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetActivityData(chapter, stage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
        }
        #endregion
    }
}
