using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI {

    public class TitleUIManagement : UIManagement {
        private TitleViewModel _viewModel;

        [SerializeField] private BoolSaveDataSO _checkFirstLoad;

        private DropdownField _saveSlotField;
        private Button _startBtn;
        private Button _exitBtn;
        
        private string slotPath = "UI/Setting/Slots/";
        private SlotSO[] slots;

        private VisualElement _fadeView;

        protected override void Start()
        {
            base.Start();

            PlayerBGM();
        }

        private async void PlayerBGM()
        {
            GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Title_Front);
            await Task.Delay(4304);
            GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Title_Back, true);
        }

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
            //_inputReaderSO.OnPressAnyKeyEvent += PressAnyKey;
        }


        protected override void OnDisable() {
            base.OnDisable();
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            //_inputReaderSO.OnPressAnyKeyEvent -= PressAnyKey;
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

            // 드롭다운 아이템들 스타일 접근
            //_saveSlotField.RegisterCallback<PointerDownEvent>(evt =>
            //{

            //    Debug.Log(root);
            //    var content = root.Q<VisualElement>(className: "unity-base-dropdown__container-outer");
            //    var test = root.Q<VisualElement>(className: "TitleUI-conteiner");
            //    Debug.Log(test);
            //    Debug.Log(content);
            //    VisualElement hoveredItem = evt.target as VisualElement;
            //    if (hoveredItem != null) {
            //        hoveredItem.style.backgroundColor = new Color(1f, 0f, 0f, 1f); // 빨간색
            //    }
            //});

            StartCoroutine(Routine());
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
        }
        private void PressAnyKey(AfterExecution execution) {
            ClickStartBtn(null);
        }
        private void ClickStartBtn(ClickEvent evt) {
            string sceneName = _checkFirstLoad.data ? "HangOutScene" : "TutorialScene";
            SaveDataEvents.SaveGameEvent?.Invoke(sceneName);
        }
        private void ChangeSlot(ChangeEvent<string> evt) {
            int index = _saveSlotField.index;
            _viewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
        }
    }
}