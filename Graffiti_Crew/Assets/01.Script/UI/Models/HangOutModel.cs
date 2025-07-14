using AH.Save;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class HangOutModel : Model {
        [SerializeField] private BoolSaveDataSO _tutorialCheck;
        public BoolSaveDataSO GetTutorialCheck() {
            return _tutorialCheck;
        }
    }
}
