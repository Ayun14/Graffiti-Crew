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

        private DialogueUIController _dialogueController;

        private void OnEnable() {
            _dialogueController = FindAnyObjectByType<DialogueUIController>();

            UIEvents.ChangeLanguageEvnet += ChangeLanguageType;
            SetDialogueLangauage();
        }
        private void OnDisable() {
            UIEvents.ChangeLanguageEvnet -= ChangeLanguageType;
        }

        private void ChangeLanguageType(LanguageType type) {
            _currentLanguageType = type;

            SetDialogueLangauage();
        }
        public static LanguageType GetLanguageType() {
            return GameManager.Instance.LanguageSystemCompo._currentLanguageType;
        }

        private void SetDialogueLangauage()
        {
            if (_currentLanguageType == LanguageType.Korea)
                _dialogueController.dialogueDataReader = _krData;
            else if (_currentLanguageType == LanguageType.English)
                _dialogueController.dialogueDataReader = _enData;
        }
    }
}
