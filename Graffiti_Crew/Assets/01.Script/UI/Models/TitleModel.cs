using AH.SaveSystem;
using System;
using UnityEngine;

namespace AH.UI.Models {
    public class TitleModel : Model {
        [Header("SaveData")]
        [SerializeField] private IntSaveDataSO _saveDataSlotIndex;

        #region SaveData
        public int GetSlotIndex() {
            return _saveDataSlotIndex.data;
        }
        public void SetSlotIndex(int index) {
            _saveDataSlotIndex.data = index;
        }
        #endregion
    }
}