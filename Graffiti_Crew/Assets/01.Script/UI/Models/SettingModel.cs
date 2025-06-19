using AH.SaveSystem;
using UnityEngine;

namespace AH.UI.Models {
    public class SettingModel : Model {
        [Header("Language")]
        [SerializeField] private LanguageSO _languageSO;
        [SerializeField] private IntSaveDataSO _languageIndexSO;
        [Header("Audio")]
        [SerializeField] private IntSaveDataSO _bgmData;
        [SerializeField] private IntSaveDataSO _sfxData;
        [Header("SaveData")]
        [SerializeField] private IntSaveDataSO _saveDataSlotIndex;

        #region SaveData
        public int GetSlotIndex() {
            return _saveDataSlotIndex.data;
        }
        public void SetSlotIndex(int index) {
            _saveDataSlotIndex.data = index;
            Debug.Log(_saveDataSlotIndex.data);
        }
        #endregion

        #region Language
        public LanguageSO GetLanguageSO() {
            return _languageSO;
        }
        public int GetLanguageIndex() {
            return _languageIndexSO.data;
        }
        public void SetLanguageIndex(int index) {
            _languageIndexSO.data = index;
        }
        public void SetBGMValue(int value) {
            _bgmData.data = value;
        }
        public void SetSFXValue(int value) {
            _sfxData.data = value;
        }
        public int GetBGMValue() {
            return _bgmData.data;
        }
        public int GetVFXValue() {
            return _sfxData.data;
        }
        #endregion
    }
}