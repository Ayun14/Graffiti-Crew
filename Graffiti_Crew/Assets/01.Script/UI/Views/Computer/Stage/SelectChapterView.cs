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
            SetSaveStageData();

            SetSaveDataToStagePoint();
        }

        private void SetSaveStageData() {
            for (int i = 1; i <= 4; i++) {
                string saveDataPath = $"SaveData/Chapter{i}/";
                List<StageSaveDataSO> list = Resources.LoadAll<StageSaveDataSO>(saveDataPath).ToList();
                _saveStageData.AddRange(list);
                _pointList[i - 1].state = _saveStageData[i - 1].stageState;
            }
        }

        //private void SetChapter() {
        //    StageSaveDataSO[] list;
        //    for (int i = 1; i <= 4; i++) {
        //        string saveDataPath = $"SaveData/Chapter{i}/";
        //        list = Resources.LoadAll<StageSaveDataSO>(saveDataPath);
        //        for(int j = 0; j <list.Length; j++ ) {
        //            if (!list[j].stageState) {
        //                break;
        //            }
        //        }
        //    }
        //}

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _pointList) {
                if (button.state != StageState.Lock) {
                    if (button.type == StageType.Stage) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn, (button.chapter, button.stage));
                    }
                    else if (button.type == StageType.Story) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn, (button.chapter, button.stage));
                    }
                    else {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickActivityBtn, (button.chapter, button.stage));
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
        //private void SetSaveDataToChapter() {
        //    StageSaveDataSO[] list;
        //    for (int i = 1; i <= 4; i++) {
        //        string saveDataPath = $"SaveData/Chapter{i}/";
        //        list = Resources.LoadAll<StageSaveDataSO>(saveDataPath);
        //        _currentChapterList[i - 1].canPlay = true;
        //        for (int j = 0; j < list.Length; j++) {
        //            if (!list[j].stageState) {
        //                _currentChapterList[i - 1].canPlay = false;
        //                break;
        //            }
        //        }
        //    }
        //}
        private void SetSaveDataToStagePoint() {
            int index = 0;
            int length = Mathf.Min(_saveStageData.Count, _pointList.Count);
            while (index < length) {
                Debug.Log(index);
                if (_saveStageData[index].stageState == StageState.Lock) {// 다음 스테이지 보이도록
                    _pointList[index].state = StageState.CanPlay; 
                    Debug.Log("break");
                    break;
                }
                _pointList[index].state = _saveStageData[index].stageState;
                //_currentPointList[index].starCount = _currentStageData[index].star;
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
