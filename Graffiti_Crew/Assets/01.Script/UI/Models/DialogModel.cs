using System;
using UnityEngine;

namespace AH.UI.Models {
    public class DialogModel : Model {
        [SerializeField] private DialogueSO _dialogueSO;

        public void SetProfile(Sprite profile) {
            _dialogueSO.profile = profile;
        }
    }
}