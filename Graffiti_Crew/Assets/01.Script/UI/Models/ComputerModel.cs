using AH.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AH.UI.Models {
    public class ComputerModel : Model {
        [SerializeField] private VisualTreeAsset _stagePointAsset;
        [SerializeField] private List<Transform> _chapter1Ratio;
        public string userName;

        public List<Transform> GetChapter1Ratio() {
            return _chapter1Ratio;
        }
        public VisualTreeAsset GetStagePointAsset() {
            return _stagePointAsset;
        }
    }
}
