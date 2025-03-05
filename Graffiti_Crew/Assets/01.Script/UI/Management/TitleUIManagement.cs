using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI
{

    public class TitleUIManagement : UIManagement
    {
        private TitleViewModel _viewModel;

        private DropdownField _saveSlotField;
        private Button _startBtn;

        private string slotPath = "UI/Setting/Slots/";
        private SlotSO[] slots;

        protected override void Init()
        {
            base.Init();
            _viewModel = new TitleViewModel(_model as TitleModel);
            slots = Resources.LoadAll<SlotSO>(slotPath);

        }

        protected override void SetupViews()
        {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            _saveSlotField = root.Q<DropdownField>("saveSlot-dropdownField");
            _startBtn = root.Q<Button>("start-btn");

            _saveSlotField.RegisterValueChangedCallback(ChangeSlot);
            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);

        }

        private void ClickStartBtn(ClickEvent evt)
        {
            SceneManager.LoadScene("HangOutScene");
        }
        private void ChangeSlot(ChangeEvent<string> evt)
        {
            int index = _saveSlotField.index;
            _viewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
        }
    }
}