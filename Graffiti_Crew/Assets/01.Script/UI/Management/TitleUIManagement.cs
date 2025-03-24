using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI {

    public class TitleUIManagement : UIManagement {
        private TitleViewModel _viewModel;

        private DropdownField _saveSlotField;
        private Button _startBtn;
        private Button _exitBtn;
        
        private string slotPath = "UI/Setting/Slots/";
        private SlotSO[] slots;

        private VisualElement _fadeView;

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
        }
        protected override void OnDisable() {
            base.OnDisable();
            PresentationEvents.FadeInOutEvent -= FadeInOut;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new TitleViewModel(_model as TitleModel);
            slots = Resources.LoadAll<SlotSO>(slotPath);
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            _saveSlotField = root.Q<DropdownField>("saveSlot-dropdownField");
            _startBtn = root.Q<Button>("start-btn");
            _exitBtn = root.Q<Button>("exit-btn");

            _fadeView = root.Q<VisualElement>("fade-view");

            _saveSlotField.RegisterCallback<PointerDownEvent>(ClickSlot);
            _saveSlotField.RegisterValueChangedCallback(ChangeSlot);
            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
            _saveSlotField.index = _viewModel.GetSlotIndex();

            StartCoroutine(Routine());
        }

        private void ClickSlot(PointerDownEvent evt) {
            _saveSlotField.schedule.Execute(() =>
            {
                Debug.Log(_saveSlotField.contentContainer);
                VisualElement popup = _uiDocument.rootVisualElement.Q<VisualElement>("unity-content-container");
                Debug.Log(popup);
                //if (popup != null) {
                //    Debug.Log("Dropdown 팝업 감지 성공! 스타일 적용");
                //    popup.style.backgroundColor = Color.white;
                //    popup.style.borderBottomLeftRadius = 5;
                //    popup.style.borderBottomRightRadius = 5;
                //}
                //else {
                //    Debug.LogWarning("Dropdown 팝업을 찾을 수 없음, 다시 시도...");
                //    _saveSlotField.schedule.Execute(() => { /* 다시 체크 */ }).StartingIn(100);
                //}
            }).StartingIn(100); // 100ms 후에 체크
        }

        private IEnumerator Routine() {
            float delayTime = 0.73f;
            
            while(true){
                yield return new WaitForSeconds(delayTime);
                _startBtn.ToggleInClassList("show-btn");
            }
        }

        private void ClickExitBtn(ClickEvent evt) {
            Application.Quit();
            Debug.Log("나가기");
        }

        private void ClickStartBtn(ClickEvent evt) {
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("HangOutScene");
        }
        private void ChangeSlot(ChangeEvent<string> evt) {
            int index = _saveSlotField.index;
            _viewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
        }
        private void FadeInOut(bool active) {
            if (active) {
                _fadeView.RemoveFromClassList("fade-out");
            }
            else {
                _fadeView.AddToClassList("fade-out");
            }
        }
    }
}