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
        private List<VisualElement> _chaptersList;
        private List<SelectChapterViewElement> _selectBtnsList;

        private VisualElement mainMap;
        private Button backToMap;
        private List<StagePointElement> _currentPointList;

        private string _saveDataPath = "SaveData/Stage/";
        private string _requestSaveDataPath = "SaveData/Request/";
        private string _storySaveDataPath = "SaveData/Story/";
        private StageSaveDataSO[] _saveStageData;
        private StageSaveDataSO[] _saveRequestData;
        private StageSaveDataSO[] _saveStoryData;

        private bool _isShowing = false;
        public SelectChapterView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
            _saveStageData = Resources.LoadAll<StageSaveDataSO>(_saveDataPath).Skip(1).ToArray(); ;
            _saveRequestData = Resources.LoadAll<StageSaveDataSO>(_requestSaveDataPath);
            _saveStoryData = Resources.LoadAll<StageSaveDataSO>(_storySaveDataPath);
            base.Initialize();
        }
        public override void Dispose() {
            base.Dispose();
        }

        protected override void SetVisualElements() {
            base.SetVisualElements();
            _chaptersList = topElement.Query<VisualElement>("unlock").ToList();
            _selectBtnsList = topElement.Query<SelectChapterViewElement>("select-btn").ToList();
            mainMap = topElement.Q<VisualElement>("main-map");
            backToMap = topElement.Q<Button>("back-map-btn");

            UnlockChapter(_chaptersList[0]);
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int i = 0;
            foreach(SelectChapterViewElement btn in _selectBtnsList) {
                btn.RegisterCallback<ClickEvent, (SelectChapterViewElement, string) >(SelectChapter, (btn, btn.chapter));
                btn.RegisterCallback<PointerEnterEvent, int>(EnterPointer, i);
                btn.RegisterCallback<PointerLeaveEvent, int>(ExitPointer, i);
                i++;
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (SelectChapterViewElement btn in _selectBtnsList) {
                btn.UnregisterCallback<ClickEvent, (SelectChapterViewElement, string)>(SelectChapter);
                btn.UnregisterCallback<PointerEnterEvent, int>(EnterPointer);
                btn.UnregisterCallback<PointerLeaveEvent, int>(ExitPointer);
            }
        }

        private void LockChapter(VisualElement element) {
            VisualElement bossName = element.Q<VisualElement>("name");
            bossName.style.display = DisplayStyle.None;
        }
        private void UnlockChapter(VisualElement element) {
            VisualElement bossName = element.Q<VisualElement>("name");
            bossName.style.display = DisplayStyle.Flex;

            VisualElement lockScreen = element.Q<VisualElement>("lock");
            lockScreen.style.display = DisplayStyle.None;
        }

        private void SelectChapter(ClickEvent evt, (SelectChapterViewElement, string) data) {
            string className = $"look-chapter{data.Item2}";
            mainMap.AddToClassList(className);
            if (!_isShowing) {
                SetStagePoints(data.Item1);
                _isShowing = true;
            }

            backToMap.RegisterCallback<ClickEvent, string>(ClickBackToMap, className);
        }
        private void ClickBackToMap(ClickEvent evt, string className) {
            mainMap.RemoveFromClassList(className);
            foreach (var button in _currentPointList) {
                button.AddToClassList("hide-point");
            }
            _isShowing = false;
            backToMap.UnregisterCallback<ClickEvent, string>(ClickBackToMap);
            UnregisterPoints();
        }
        private void SetStagePoints(SelectChapterViewElement topElement) {
            _currentPointList = topElement.Query<StagePointElement>(className: "stage-point").ToList();
            SetSaveDataToStagePoint();
            foreach (var button in _currentPointList) {
                button.RemoveFromClassList("hide-point");
                if (button.canPlay) {
                    if (button.type == StageType.Stage) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn, (button.chapter, button.stage));
                    }
                    else if (button.type == StageType.Story) {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn, (button.chapter, button.stage));
                    }
                    else {
                        button.RegisterCallback<ClickEvent, (string chapter, string stage)>(ClickRequestBtn, (button.chapter, button.stage));
                    }
                }
            }
        }
        private void UnregisterPoints() {
            foreach (var button in _currentPointList) {
                if (button.canPlay) {
                    if (button.type == StageType.Stage) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStageBtn);
                    }
                    else if (button.type == StageType.Story) {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickStoryBtn);
                    }
                    else {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage)>(ClickRequestBtn);
                    }
                }
            }
        }
        private void SetSaveDataToStagePoint() {
            int index = 0;
            int stageIndex = 0;
            int storyIndex = 0;
            int requestIndex = 0;

            while (index <= _currentPointList.Count - _saveRequestData.Length) {
                if (_currentPointList[index].type == StageType.Stage) {
                    if (!_saveStageData[stageIndex].isClear) {
                        _currentPointList[index].canPlay = true; // 다음 스테이지 보이도록
                        break;
                    }
                    _currentPointList[index].canPlay = _saveStageData[stageIndex].isClear;
                    _currentPointList[index].starCount = _saveStageData[stageIndex].star;
                    stageIndex++;
                }
                else if (_currentPointList[index].type == StageType.Story) {
                    if (!_saveStoryData[storyIndex].isClear) {
                        _currentPointList[index].canPlay = true; // 다음 스테이지 보이도록
                        break;
                    }
                    _currentPointList[index].canPlay = _saveStoryData[storyIndex].isClear;
                    _currentPointList[index].starCount = _saveStoryData[storyIndex].star;
                    storyIndex++;
                }
                else if (_currentPointList[index].type == StageType.Request) {
                    if (!_saveRequestData[requestIndex].isClear) {
                        _currentPointList[index].canPlay = true; // 다음 스테이지 보이도록
                        break;
                    }
                    _currentPointList[index].canPlay = _saveRequestData[requestIndex].isClear;
                    _currentPointList[index].starCount = _saveRequestData[requestIndex].star;
                    requestIndex++;
                }
                index++;
            }
        }

        #region ClickStages
        private void ClickStageBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Stage{data.stage}";
            Debug.Log($"Chapter{data.chapter} | Stage{data.stage}");
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
        private void ClickRequestBtn(ClickEvent evt, (string chapter, string stage) data) {
            string chapter = $"Chapter{data.chapter}";
            string stage = $"Request{data.stage}";

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetRequestData(chapter, stage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
        }
        #endregion
        private void EnterPointer(PointerEnterEvent evt, int index) {
            VisualElement element = _chaptersList[index];
            Color currentColor = element.resolvedStyle.unityBackgroundImageTintColor;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0.2f);
            element.style.unityBackgroundImageTintColor = newColor;
        }
        private void ExitPointer(PointerLeaveEvent evt, int index) {
            VisualElement element = _chaptersList[index];
            Color currentColor = element.resolvedStyle.unityBackgroundImageTintColor;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            element.style.unityBackgroundImageTintColor = newColor;
        }
    }
}
