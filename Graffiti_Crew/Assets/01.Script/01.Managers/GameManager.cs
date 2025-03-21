using AH.SaveSystem;
using AH.UI.Events;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    public static SlotSO currentSlot;

    private string slotPath = "UI/Setting/Slots/";
    private SlotSO[] slots;
    public IntSaveDataSO slotIndex;

    #region Systems

    public SoundManager SoundSystemCompo { get; private set; }
    public LanguageSystem LanguageSystemCompo { get; private set; }

    #endregion

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }

        SoundSystemCompo = GetComponentInChildren<SoundManager>();
        LanguageSystemCompo = GetComponentInChildren<LanguageSystem>();
    }
    private void OnEnable() {
        UIEvents.ChangeSlotEvent += ChangeSlot;
    }
    private void OnDisable() {
        UIEvents.ChangeSlotEvent -= ChangeSlot;
    }
    public static void SetSlot() {
        instance.slots = Resources.LoadAll<SlotSO>(instance.slotPath);
        currentSlot = instance.slots[instance.slotIndex.data];
    }
    private void ChangeSlot(SlotSO slot) {
        currentSlot = slot;
    }
}
