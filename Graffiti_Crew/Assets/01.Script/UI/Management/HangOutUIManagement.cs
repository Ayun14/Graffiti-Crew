using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI {
    public class HangOutUIManagement : UIManagement {
        private HangOutViewModel _viewModel;

        private DialougeView _dialougeView;
        private SettingView _settingView;

        private Button _settingBtn;
        private Button _closeBtn;

        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
        }

        protected override void Init() {
            base.Init();
            _viewModel = new HangOutViewModel(_model as HangOutModel);
        }

        protected override void SetupViews() {
            base.SetupViews();
            VisualElement root = _uiDocument.rootVisualElement;

            _dialougeView = new DialougeView(root.Q<VisualElement>("DialogBoxView"), _viewModel);
            _settingView = new SettingView(root.Q<VisualElement>("SettingView"), _viewModel);
            
            _settingBtn = root.Q<Button>("setting-btn");
            _settingBtn.RegisterCallback<ClickEvent>(ClickSettingBtn);
            _closeBtn = root.Q<Button>("close-btn");
            _closeBtn.RegisterCallback<ClickEvent>(ClickCloseBtn);
        }

        private void ClickCloseBtn(ClickEvent evt) {
            _settingView.Hide();
        }

        private void ClickSettingBtn(ClickEvent evt) {
            _settingView.Show();
        }
        private void ShowDialougeView(bool active) {
            if (active) {
                _dialougeView.Show();
            }
            else {
                _dialougeView.Hide();
            }
        }
    }
}
