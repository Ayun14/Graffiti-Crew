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

            // 드롭다운 아이템들 스타일 접근
//            _saveSlotField.RegisterCallback<PointerDownEvent>(evt =>
//            {
//#if UNITY_EDITOR
//                // 에디터에서만 지연 호출 사용
//                UnityEditor.EditorApplication.delayCall += () =>
//                {
//                    StyleDropdownItems();
//                };
//#else
//        // 빌드 환경에서는 다음 프레임에서 실행하기 위해 코루틴 사용
//        StartCoroutine(StyleDropdownItemsNextFrame());
//#endif
//            });

            StartCoroutine(Routine());
        }

        // 드롭다운 아이템 스타일링을 위한 별도 메서드
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

        // 빌드 환경에서 다음 프레임에 스타일 적용을 위한 코루틴
        private IEnumerator StyleDropdownItemsNextFrame()
        {
            // 한 프레임 대기
            yield return null;

            // 스타일 적용
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