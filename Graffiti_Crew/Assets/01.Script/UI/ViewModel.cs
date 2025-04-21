using UnityEngine;

namespace AH.UI.ViewModels { 
    public abstract class ViewModel {
        public virtual LanguageSO GetLanguageSO() {
            return null;
        }
        public virtual int GetLanguageIndex() {
            return 0;
        }
        public virtual void SetLanguageIndex(int index) {

        }
        public virtual void SetBGMValue(int value) {

        }
        public virtual void SetSFXValue(int value) {

        }
        public virtual int GetBGMValue() {
            return 0;
        }
        public virtual int GetVFXValue() {
            return 0;
        }
    }
}