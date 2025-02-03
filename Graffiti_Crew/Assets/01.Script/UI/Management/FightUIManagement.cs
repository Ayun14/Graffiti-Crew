using AH.UI.Events;
using AH.UI.Models;
using AH.UI.ViewModels;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace AH.UI {
    public class FightUIManagement : UIManagement {
        private FightView _fightView;

        private FightViewModel _viewModel;


        protected override void Awake() {
            base.Awake();
            Init();
            SetupViews();
        }

        private void Init() {
            _uiDocument = GetComponent<UIDocument>();
            _viewModel = new FightViewModel(_model as FightModel);
        }
        private void SetupViews() {
            VisualElement root = _uiDocument.rootVisualElement;

            _fightView = new FightView(root.Q<VisualElement>("FightView"), _viewModel);
            
            _fightView.Show();
        }
    }
}