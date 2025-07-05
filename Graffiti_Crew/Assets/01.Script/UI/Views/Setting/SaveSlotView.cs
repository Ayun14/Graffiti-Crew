using AH.Save;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static AH.Save.SaveSystem;

namespace AH.UI.Views {
    public class SaveSlotView : UIView {
        private TitleViewModel _titleViewModel;

        private List<VisualElement> _slotList;
        private List<Button> _resetBtns;
        private Button _closeBtn;
        private SlotSO[] _slotsBtns;
        private string _slotPath = "UI/Setting/Slots/";

        public SaveSlotView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            _slotsBtns = Resources.LoadAll<SlotSO>(_slotPath);
            _titleViewModel = viewModel as TitleViewModel;
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();

            _closeBtn = topElement.Q<Button>("slot-close-btn");
            _slotList = topElement.Query<VisualElement>(className: "slot-btn").ToList();
            _resetBtns = topElement.Query<Button>(className: "reset-btn").ToList();
            SetupSlotBtn();
        }
        private void SetupSlotBtn() {
            string[] timeDatas = SaveHelperSystem.FindAllDataValue("LastPlayTime", "SaveGameDataFile.txt");
            string[] ProgressrDatas = SaveHelperSystem.FindAllDataValue("LastProgress", "SaveGameDataFile.txt");
            Label lastPlayTimeLabel = null;
            Label lastChapterLabel = null;

            for(int i = 0; i < _slotList.Count; i++) {
                lastPlayTimeLabel = _slotList[i].Q<Label>("play-time-txt");
                lastChapterLabel = _slotList[i].Q<Label>("game-progress-txt");

                if(timeDatas[i] == "") {
                    lastPlayTimeLabel.text = "새 파일";
                    lastChapterLabel.text = "";
                }
                else {
                    lastPlayTimeLabel.text = timeDatas[i];
                    if(ProgressrDatas[i] == "") { // 기존에 플레이는 한적 있는데 스테이지를 안들어간 경우
                        lastChapterLabel.text = "플레이 정보 없음";
                    }
                    else {
                        lastChapterLabel.text = ProgressrDatas[i];
                    }
                }
            }
        }
        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseView);

            for(int i = 0; i < _slotList.Count; i++) {
                _slotList[i].RegisterCallback<ClickEvent, int>(ChangeSlot, i);
                _resetBtns[i].RegisterCallback<ClickEvent, (VisualElement, int)>(ResetSlot, (_slotList[i], i));
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            _closeBtn.UnregisterCallback<ClickEvent>(ClickCloseView);

            for (int i = 0; i < _slotList.Count; i++) {
                _slotList[i].UnregisterCallback<ClickEvent, int>(ChangeSlot);
                _resetBtns[i].UnregisterCallback<ClickEvent, (VisualElement, int)>(ResetSlot);
            }
        }

        private void ChangeSlot(ClickEvent evt, int index) {
            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);

            _titleViewModel.SetSlotIndex(index);
            SlotSO selectSlot = _slotsBtns[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);

            TitleEvent.StartGameEvent?.Invoke();
        }
        private void ResetSlot(ClickEvent evt, (VisualElement button, int index) data) {
            Label lastPlayTimeLabel = data.button.Q<Label>("play-time-txt");
            Label lastChapterLabel = data.button.Q<Label>("game-progress-txt");
            lastPlayTimeLabel.text = "새 파일";
            lastChapterLabel.text = "";

            SaveDataEvents.DeleteSaveDataEvent?.Invoke(data.index);
            evt.StopPropagation();
        }
        private void ClickCloseView(ClickEvent evt) {
            StageEvent.HideViewEvent?.Invoke();
        }
    }
}
