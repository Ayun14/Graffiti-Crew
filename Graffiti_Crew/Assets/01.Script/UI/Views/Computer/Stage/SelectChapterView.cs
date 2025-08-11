using AH.Save;
using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views
{
    public class SelectChapterView : UIView
    {
        private ComputerViewModel ComputerViewModel;

        private List<StagePointElement> _pointList;
        private List<StageSaveDataSO> _saveStageData = new List<StageSaveDataSO>();

        private VisualElement _map;
        private string _selectStageName;

        public SelectChapterView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel)
        {
        }

        public override void Initialize()
        {
            ComputerViewModel = viewModel as ComputerViewModel;
            hideOnAwake = false;

            base.Initialize();
            ComputerEvent.CloseDescriptionEvent += UnforceSelectStage;
            ComputerEvent.HideViewEvent += UnforceSelectStage;
        }
        public override void Dispose()
        {
            ComputerEvent.CloseDescriptionEvent -= UnforceSelectStage;
            ComputerEvent.HideViewEvent -= UnforceSelectStage;
            base.Dispose();
        }
        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            _map = topElement.Q<VisualElement>("main-map");
            _pointList = topElement.Query<StagePointElement>(className: "stage-point").ToList();
            _saveStageData = ComputerViewModel.GetSaveStageDatas();
            LoadSaveData();

            SetSaveDataToStagePoint();
        }

        private void LoadSaveData()
        {
            for (int i = 0; i < 3; i++)
            {
                //_pointList[i].StageType = _saveStageData[i].stageType;
                _pointList[i].StageState = _saveStageData[i].stageState;
            }
        }
        private void SetSaveDataToStagePoint()
        {
            int index = 0;
            int length = Mathf.Min(_saveStageData.Count, _pointList.Count);

            for (int i = 0; i < length; i++) { // 일단 다 끄고
                _pointList[i].StageState = StageState.Lock;
            }

            while (index < length) // 플레이 가능 스테이지만 열기
            {
                if (_saveStageData[index].stageState == StageState.CanPlay || _saveStageData[index].stageState == StageState.Lock) { 
                    _pointList[index].StageState = StageState.CanPlay;
                    break;
                }
                _pointList[index].StageState = StageState.Clear;
                _pointList[index].StageState = _saveStageData[index].stageState;
                index++;
            }
        }

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();
            foreach (var button in _pointList)
            {
                if (button.StageState != StageState.Lock)
                {
                    button.RegisterCallback<MouseEnterEvent>(evt =>
                    {
                        button.AddToClassList("unlock");
                    });
                    button.RegisterCallback<MouseLeaveEvent>(evt =>
                    {
                        button.RemoveFromClassList("unlock");
                    });
                    switch (button.StageType)
                    {
                        case StageType.Battle:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickBattleBtn, (button.chapter, button.stage, button.imageNumber));
                            break;
                        case StageType.Activity:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickActivityBtn, (button.chapter, button.stage, button.imageNumber));
                            break;
                        case StageType.Story:
                            button.RegisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickStoryBtn, (button.chapter, button.stage, button.imageNumber));
                            break;
                    }
                }
            }
        }
        protected override void UnRegisterButtonCallbacks()
        {
            base.UnRegisterButtonCallbacks();
            foreach (var button in _pointList)
            {
                if (button.StageState != StageState.Lock)
                {
                    button.UnregisterCallback<MouseEnterEvent>(evt =>
                    {
                        button.AddToClassList("unlock");
                    });
                    button.UnregisterCallback<MouseLeaveEvent>(evt =>
                    {
                        button.RemoveFromClassList("unlock");
                    });
                    if (button.StageType == StageType.Battle)
                    {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickBattleBtn);
                    }
                    else if (button.StageType == StageType.Story)
                    {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickStoryBtn);
                    }
                    else
                    {
                        button.UnregisterCallback<ClickEvent, (string chapter, string stage, string stagenumber)>(ClickActivityBtn);
                    }
                }
            }
        }

        #region ClickStages
        private void ClickBattleBtn(ClickEvent evt, (string chapter, string stage, string stagenumber) data)
        {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            string chapter = $"Chapter{data.chapter}";
            string stage = $"Battle{data.stage}";
            string path = $"StageData/{chapter}/{stage}";

            StageDataSO stageData = Resources.Load<StageDataSO>(path);

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStageData(chapter, stage, StageType.Battle, data.stagenumber);
            SetDescription(stageData, data);
        }
        private void ClickStoryBtn(ClickEvent evt, (string chapter, string stage, string stageNumber) data)
        {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            string chapter = $"Chapter{data.chapter}";
            string stage = $"Story{data.stage}";
            string path = $"StageData/{chapter}/{stage}";

            StageDataSO stageData = Resources.Load<StageDataSO>(path);
            ComputerViewModel.SetStageData(chapter, stage, StageType.Story, data.stageNumber);
            SetDescription(stageData, data);
        }
        private void ClickActivityBtn(ClickEvent evt, (string chapter, string stage, string stagenumber) data)
        {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            string chapter = $"Chapter{data.chapter}";
            string stage = $"Activity{data.stage}";
            string path = $"StageData/{chapter}/{stage}";

            StageDataSO stageData = Resources.Load<StageDataSO>(path);

            ComputerEvent.SelectStageEvent?.Invoke(chapter, stage);
            ComputerViewModel.SetStageData(chapter, stage, StageType.Activity, data.stagenumber);
            SetDescription(stageData, data);
        }
        private void SetDescription(StageDataSO stageData, (string chapter, string stage, string stagenumber) data)
        {
            ComputerEvent.SelectStageEvent?.Invoke(stageData.nextChapter, stageData.nextStage);
            ComputerEvent.ShowStageDescriptionViewEvent?.Invoke();
            ForceSelectStage(stageData, data);
        }
        #endregion
        private void ForceSelectStage(StageDataSO stageData, (string chapter, string stage, string stageNumber) data)
        {
            if(stageData.stageType == StageType.Story) {
                _selectStageName = $"{stageData.nextChapter}{stageData.nextStage}";
            }
            else if(stageData.stageType == StageType.Battle) {
                _selectStageName = $"Chapter{data.chapter}Battle{data.stage}";
            }
            else {
                _selectStageName = $"Chapter{data.chapter}Activity{data.stage}";
            }
            _map.AddToClassList(_selectStageName);
        }
        private void UnforceSelectStage()
        {
            _map.RemoveFromClassList(_selectStageName);
        }
    }
}
