using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using Unity.VisualScripting;
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

            _saveSlotField.RegisterValueChangedCallback(ChangeSlot);
            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);
            _saveSlotField.index = _viewModel.GetSlotIndex();
        }
        private void ClickExitBtn(ClickEvent evt) {
            Application.Quit();
            Debug.Log("³ª°¡±â");
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