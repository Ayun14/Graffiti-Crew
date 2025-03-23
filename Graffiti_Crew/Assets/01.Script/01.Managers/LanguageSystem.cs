using AH.UI.Events;
using AH.UI.Views;
using System;
using UnityEngine;

public class LanguageSystem : MonoBehaviour {
    public static event Action<LanguageType> LanguageChangedEvent;
    public LanguageType _currentLanguageType = LanguageType.Korea;

    private void OnEnable() {
        UIEvents.ChangeLanguageEvnet += ChangeLanguageType;
    }
    private void OnDisable() {
        UIEvents.ChangeLanguageEvnet -= ChangeLanguageType;
    }

    private void ChangeLanguageType(LanguageType type) {
        _currentLanguageType = type;
        LanguageChangedEvent?.Invoke(_currentLanguageType);
    }
    public static LanguageType GetLanguageType() {
        return GameManager.Instance.LanguageSystemCompo._currentLanguageType;
    }
}
