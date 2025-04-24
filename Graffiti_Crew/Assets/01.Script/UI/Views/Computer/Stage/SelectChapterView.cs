using AH.UI.CustomElement;
using AH.UI.Events;
using AH.UI.ViewModels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class SelectChapterView : UIView {
        private List<VisualElement> _chaptersList;
        private List<SelectChapterViewElement> _selectBtnsList;

        private VisualElement mainMap;
        private Button backToMap;
        
        public SelectChapterView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }
        public override void Initialize() {
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

            foreach(VisualElement map in _chaptersList) {
                UnlockChapter(map);
            }
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
            foreach(SelectChapterViewElement btn in _selectBtnsList) {
                btn.RegisterCallback<ClickEvent, (SelectChapterViewElement, string) >(SelectChapter, (btn, btn.chapter));
            }
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
            foreach (SelectChapterViewElement btn in _selectBtnsList) {
                btn.UnregisterCallback<ClickEvent, (SelectChapterViewElement, string)>(SelectChapter);
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

            

            backToMap.RegisterCallback<ClickEvent, string>(ClickBackToMap, className);
        }

        private void ClickBackToMap(ClickEvent evt, string className) {
            mainMap.RemoveFromClassList(className);
            backToMap.UnregisterCallback<ClickEvent, string>(ClickBackToMap);
        }
    }
}
