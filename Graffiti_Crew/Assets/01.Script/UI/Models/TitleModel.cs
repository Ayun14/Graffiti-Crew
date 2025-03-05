using AH.SaveSystem;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class TitleModel : Model {
        [SerializeField] private IntSaveDataSO slotIndex;

        public void SetSlotIndex(int index) {
            slotIndex.data = index;
        }
    }
}