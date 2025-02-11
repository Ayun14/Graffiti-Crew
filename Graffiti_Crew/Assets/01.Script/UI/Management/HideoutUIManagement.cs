using AH.UI.Events;
using AH.UI.Views;
using UnityEngine;

namespace AH.UI {
    public class HideoutUIManagement : UIManagement {
        private DialougeView _dialougeView;
        protected override void OnEnable() {
            base.OnEnable();
            DialougeEvent.ShowDialougeViewEvent += ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent += HideDialougeView;
        }
        protected override void OnDisable() {
            base.OnDisable();
            DialougeEvent.ShowDialougeViewEvent -= ShowDialougeView;
            DialougeEvent.HideDialougeViewEvent -= HideDialougeView;
        }

        private void ShowDialougeView() {
            _dialougeView.Show();
        }
        private void HideDialougeView() {
            _dialougeView.Hide();
        }
    }
}
