using AH.UI.Models;
using UnityEngine;

namespace AH.UI.ViewModels {
    public class SettingViewModel : ViewModel {
        private SettingModel _settingModel;
        public int currentBtnIndex = -1;

        public SettingViewModel(SettingModel settingModel) {
            _settingModel = settingModel;
        }
        #region SaveData
        public int GetSlotIndex() {
            return _settingModel.GetSlotIndex();
        }
        public void SetSlotIndex(int index) {
            Debug.Log(index);
            _settingModel.SetSlotIndex(index);
        }
        #endregion
        #region Lenguage
        public LanguageSO GetLanguageSO() {
            return _settingModel.GetLanguageSO();
        }
        public int GetLanguageIndex() {
            return _settingModel.GetLanguageIndex();
        }
        public void SetLanguageIndex(int index) {
            _settingModel.SetLanguageIndex(index);
        }
        public void SetBGMValue(int value) {
            _settingModel.SetBGMValue(value);
        }
        public void SetSFXValue(int value) {
            _settingModel.SetSFXValue(value);
        }
        public int GetBGMValue() {
            return _settingModel.GetBGMValue();
        }
        public int GetVFXValue() {
            return _settingModel.GetVFXValue();
        }
        #endregion
    }
}
