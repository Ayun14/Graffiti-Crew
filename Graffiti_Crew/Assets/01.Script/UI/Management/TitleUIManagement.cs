using AH.SaveSystem;
using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
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

            StartCoroutine(PlayerBGM());
        }

        private IEnumerator PlayerBGM()
        {
            AudioSource source = GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Title_Front);
            yield return new WaitForSeconds(source.clip.length - 0.85f);
            GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Title_Back);
        }

        protected override void OnEnable() {
            base.OnEnable();
            PresentationEvents.FadeInOutEvent += FadeInOut;
            _inputReaderSO.OnPressAnyKeyEvent += PressAnyKey;
        }
        protected override void OnDisable() {
            base.OnDisable();
            PresentationEvents.FadeInOutEvent -= FadeInOut;
            _inputReaderSO.OnPressAnyKeyEvent -= PressAnyKey;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new TitleViewModel(_model as TitleModel);
            slots = Resources.LoadAll<SlotSO>(slotPath);
        }
        protected override void SetupViews()
        {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;
            //_saveSlotField = root.Q<DropdownField>("saveSlot-dropdownField");
            _startBtn = root.Q<Button>("start-btn");
            _exitBtn = root.Q<Button>("exit-btn");
            _fadeView = root.Q<VisualElement>("fade-view");
            _saveSlotField.RegisterValueChangedCallback(ChangeSlot);
            _startBtn.RegisterCallback<ClickEvent>(ClickStartBtn);
            _exitBtn.RegisterCallback<ClickEvent>(ClickExitBtn);


            //_saveSlotField.index = _viewModel.GetSlotIndex();

            // ��Ӵٿ� �����۵� ��Ÿ�� ����
//            _saveSlotField.RegisterCallback<PointerDownEvent>(evt =>
//            {
//#if UNITY_EDITOR
//                // �����Ϳ����� ���� ȣ�� ���
//                UnityEditor.EditorApplication.delayCall += () =>
//                {
//                    StyleDropdownItems();
//                };
//#else
//        // ���� ȯ�濡���� ���� �����ӿ��� �����ϱ� ���� �ڷ�ƾ ���
//        StartCoroutine(StyleDropdownItemsNextFrame());
//#endif
//            });

            StartCoroutine(Routine());
        }

        // ��Ӵٿ� ������ ��Ÿ�ϸ��� ���� ���� �޼���
        private void StyleDropdownItems()
        {
            var content = UIDocument.rootVisualElement.parent.panel.visualTree.Q<VisualElement>(className: "unity-base-dropdown");
            if (content != null)
            {
                List<VisualElement> list = content.Query<VisualElement>(className: "unity-base-dropdown__item").ToList();
                foreach (VisualElement item in list)
                {
                    item.RegisterCallback<PointerEnterEvent>(evt =>
                    {
                        item.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
                    });
                    item.RegisterCallback<PointerLeaveEvent>(evt =>
                    {
                        item.style.backgroundColor = new Color(0f, 0f, 0f, 1f);
                    });
                }
            }
        }

        // ���� ȯ�濡�� ���� �����ӿ� ��Ÿ�� ������ ���� �ڷ�ƾ
        private IEnumerator StyleDropdownItemsNextFrame()
        {
            // �� ������ ���
            yield return null;

            // ��Ÿ�� ����
            StyleDropdownItems();
        }
        private async void Fade() {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            string sceneName = _checkFirstLoad.data ? "HangOutScene" : "TutorialScene";
            SaveDataEvents.SaveGameEvent?.Invoke(sceneName);
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
            Fade();
        }
        private void ChangeSlot(ChangeEvent<string> evt) {
            int index = _saveSlotField.index;
            _viewModel.SetSlotIndex(index);
            SlotSO selectSlot = slots[index];
            UIEvents.ChangeSlotEvent?.Invoke(selectSlot);
        }
    }
}