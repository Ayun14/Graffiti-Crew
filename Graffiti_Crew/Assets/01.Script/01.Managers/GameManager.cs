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

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Start() {
        slots = Resources.LoadAll<SlotSO>(slotPath);
        currentSlot = slots[slotIndex.data];
    }
    private void OnEnable() {
        UIEvents.ChangeSlotEvent += ChangeSlot;
    }
    private void OnDisable() {
        UIEvents.ChangeSlotEvent -= ChangeSlot;
    }

    private void ChangeSlot(SlotSO slot) {
        currentSlot = slot;
    }
}
