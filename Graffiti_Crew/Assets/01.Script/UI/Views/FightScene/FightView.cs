using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Views {
    public class FightView : UIView {
        private ProgressBar _rivalProgress;
        private ProgressBar _playerProgress;
        private ProgressBar _sprayAmountProgress;
        private ProgressBar _sprayTotalAmountProgress;
        public FightView(VisualElement topContainer, ViewModel viewModel) : base(topContainer, viewModel) {
        }

        public override void Initialize() {
            base.Initialize();
        }
        protected override void SetVisualElements() {
            base.SetVisualElements();
            _rivalProgress = topElement.Q<ProgressBar>("rival-progress");
            _playerProgress = topElement.Q<ProgressBar>("player-progress");
            _sprayAmountProgress = topElement.Q<ProgressBar>("spray-amount-progress");
            _sprayTotalAmountProgress = topElement.Q<ProgressBar>("spray-total-amount-progress");
        }

        protected override void RegisterButtonCallbacks() {
            base.RegisterButtonCallbacks();
        }
        protected override void UnRegisterButtonCallbacks() {
            base.UnRegisterButtonCallbacks();
        }
    }
}
