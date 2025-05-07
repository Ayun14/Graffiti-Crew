using AH.SaveSystem;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using NUnit.Framework;
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
        private List<SelectChapterViewElement> _currentChapterList;
        private StageSaveDataSO[] _currentStageData;


        private bool _isShowing = false;
        public SelectChapterView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            ComputerViewModel = viewModel as ComputerViewModel;
           
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

            _currentChapterList = topElement.Query<SelectChapterViewElement>("select-btn").ToList();

            SetChapter();
        }

        private void SetChapter() {
            StageSaveDataSO[] list;
            for (int i = 1; i <= 4; i++) {
                string saveDataPath = $"SaveData/Chapter{i}/";
                list = Resources.LoadAll<StageSaveDataSO>(saveDataPath);
                for(int j = 0; j <list.Length; j++ ) {
                    if (!list[j].isClear) {
                        LockChapter(_chaptersList[i - 1]); // 있으면 잠구기
                        break;
                    }
                }
            }
            UnlockChapter(_chaptersList[0]);
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            int i = 0;
            foreach(SelectChapterViewElement btn in _selectBtnsList) {
                if (btn.canPlay) {
                    Debug.Log("true");
                    btn.RegisterCallback<ClickEvent, (SelectChapterViewElement, string)>(SelectChapter, (btn, btn.chapter));
                    btn.RegisterCallback<PointerEnterEvent, int>(EnterPointer, i);
                    btn.RegisterCallback<PointerLeaveEvent, int>(ExitPointer, i);
                }
                i++;
            }
            backToMap.RegisterCallback<ClickEvent>(ClickBackBtn);
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
            string _saveDataPath = $"SaveData/Chapter{data.Item2}/";
            _currentStageData = Resources.LoadAll<StageSaveDataSO>(_saveDataPath);

            string className = $"look-chapter{data.Item2}";
            mainMap.AddToClassList(className);
            if (!_isShowing) {
                SetStagePoints(data.Item1);
                _isShowing = true;
            }

            backToMap.UnregisterCallback<ClickEvent>(ClickBackBtn);
            backToMap.RegisterCallback<ClickEvent, string>(ClickBackToMap, className);
        }
        private void ClickBackToMap(ClickEvent evt, string className) {
            mainMap.RemoveFromClassList(className);
            foreach (var button in _currentPointList) {
                button.AddToClassList("hide-point");
            }
            _isShowing = false;
            backToMap.UnregisterCallback<ClickEvent, string>(ClickBackToMap);
            backToMap.RegisterCallback<ClickEvent>(ClickBackBtn);
            UnRegisterPoints();
        }
        private void ClickBackBtn(ClickEvent evt) {
            ComputerEvent.HideViewEvent?.Invoke();
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
        private void UnRegisterPoints() {
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
        private void SetSaveDataToChapter() {
            StageSaveDataSO[] list;
            for (int i = 1; i <= 4; i++) {
                string saveDataPath = $"SaveData/Chapter{i}/";
                list = Resources.LoadAll<StageSaveDataSO>(saveDataPath);
                _currentChapterList[i - 1].canPlay = true;
                for (int j = 0; j < list.Length; j++) {
                    if (!list[j].isClear) {
                        _currentChapterList[i - 1].canPlay = false;
                        break;
                    }
                }
            }
        }
        private void SetSaveDataToStagePoint() {
            int index = 0;

            while (index < 3) {
                if (!_currentStageData[index].isClear) {
                    _currentPointList[index].canPlay = true; // 다음 스테이지 보이도록
                    break;
                }
                _currentPointList[index].canPlay = _currentStageData[index].isClear;
                _currentPointList[index].starCount = _currentStageData[index].star;
                index++;
            }
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
