using AH.UI.Events;
using AH.UI.Views;
using System;
using UnityEngine;

public class LanguageSystem : MonoBehaviour {
    private static LanguageSystem instance;
    public static event Action<LanguageType> LanguageChangedEvent;
    private LanguageType _currentLanguageType = LanguageType.Korea;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
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
        return instance._currentLanguageType;
    }
}
