using AH.UI.Events;
using AH.UI.Views;
using System.Collections.Generic;
using UnityEngine;

namespace AH.LanguageSystem {
    public class LanguageSystem : MonoBehaviour {
        [SerializeField] private LanguageType _currentLanguageType = LanguageType.Korea;
        [SerializeField] private List<LanguageIndexSO> _list;

        [SerializeField] private DialogueDataReader _krData;
        [SerializeField] private DialogueDataReader _enData;

        private void OnEnable() {
            UIEvents.ChangeLanguageEvnet += ChangeLanguageType;
        }
        private void OnDisable() {
            UIEvents.ChangeLanguageEvnet -= ChangeLanguageType;
        }

        private void ChangeLanguageType(LanguageType type) {
            _currentLanguageType = type;

        }
        public static LanguageType GetLanguageType() {
            return GameManager.Instance.LanguageSystemCompo._currentLanguageType;
        }
    }
}
