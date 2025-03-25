using AH.SaveSystem;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VolumeComponent;

namespace AH.UI.Views {
    public class SelectStageView : UIView {
        private ComputerViewModel ComputerViewModel;

        private List<StagePointElement> _stagePointList;

        private Button _exitBtn;

        private string _saveDataPath = "SaveData/Stage/";
        private string _requestSaveDataPath = "SaveData/Request/";
        private string _storySaveDataPath = "SaveData/Story/";
        private StageSaveDataSO[] _saveStageData;
        private StageSaveDataSO[] _saveRequestData;
        private StageSaveDataSO[] _saveStoryData;

        public SelectStageView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _saveStageData = Resources.LoadAll<StageSaveDataSO>(_saveDataPath).Skip(1).ToArray(); ;
            _saveRequestData = Resources.LoadAll<StageSaveDataSO>(_requestSaveDataPath);
            _saveStoryData = Resources.LoadAll<StageSaveDataSO>(_storySaveDataPath);
            
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _stagePointList = topElement.Query<StagePointElement>(className : "stage-point").ToList();
            
            _exitBtn = topElement.Q<Button>("exit-btn");
        }

        public override void Show() {
            SetStagePoint();
            SetStar();
            RegisterButtonCallbacks();
            base.Show();
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                if (button.canPlay) {
                    if(button.type == StageType.Stage) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn, (button.chapter, button.stage));
                    }
                    else if(button.type == StageType.Story) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn, (button.chapter, button.stage));
                    }
                    else {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickRequestBtn, (button.chapter, button.stage));
                    }
                }
            }
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _stagePointList) {
                if (button.canPlay) {
                    if (button.type == StageType.Stage) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn);
                    }
                    else {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickRequestBtn);
                    }
                }
            }
            _exitBtn.UnregisterCallback<ClickEvent>(ClickExitBtn);
        }

        private void ClickExitBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
        }

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

            ComputerViewModel.SetStoryData(chapter, stage);
            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("StoryScene");
        }
        private void ClickRequestBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Request{data.stage}";

            ComputerViewModel.SetRequestData(chapter, stage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
        }

        private void SetStar() {
            int index = 0;
            int storyIndex = 0;
            int stageIndex = 0;
            int requestIndex = 0;
            while (index <= _stagePointList.Count - _saveRequestData.Length) {
                if (_stagePointList[index].type == StageType.Stage) {
                    _stagePointList[index].starCount = _saveStageData[storyIndex++].star;
                }
                else if (_stagePointList[index].type == StageType.Story) {
                    _stagePointList[index].starCount = _saveStoryData[stageIndex++].star;
                }
                else {
                    _stagePointList[index].starCount = _saveRequestData[requestIndex++].star;
                }
                index++;
            }
        }
        private void SetStagePoint() {
            int index = 0;
            int stageIndex = 0;
            int storyIndex = 0;
            int requestIndex = 0;
            while (index <= _stagePointList.Count - _saveRequestData.Length) {
                if (_stagePointList[index].type == StageType.Stage) {
                    if (!_saveStageData[stageIndex].isClear) { // 다음 스테이지 보이도록
                        _stagePointList[index].canPlay = true;
                        break;
                    }
                    _stagePointList[index].canPlay = _saveStageData[stageIndex++].isClear;
                }
                else if (_stagePointList[index].type == StageType.Story) {
                    if (!_saveStoryData[storyIndex].isClear) { // 다음 스테이지 보이도록
                        _stagePointList[index].canPlay = true;
                        break;
                    }
                    _stagePointList[index].canPlay = _saveStoryData[storyIndex++].isClear;
                }
                else {
                    if (!_saveRequestData[requestIndex].isClear) { // 다음 스테이지 보이도록
                        _stagePointList[index].canPlay = true;
                        break;
                    }
                    _stagePointList[index].canPlay = _saveRequestData[requestIndex++].isClear;
                }
                index++;
            }
        }
    }
}
