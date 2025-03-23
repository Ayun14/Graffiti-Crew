using AH.SaveSystem;
using AH.UI.Events;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {
    public static SlotSO currentSlot;

    private string slotPath = "UI/Setting/Slots/";
    private SlotSO[] slots;
    public IntSaveDataSO slotIndex;

    #region Systems

    public SoundManager SoundSystemCompo { get; private set; }
    public LanguageSystem LanguageSystemCompo { get; private set; }

    #endregion

    protected override void Awake() {
        base.Awake();

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
        Instance.slots = Resources.LoadAll<SlotSO>(Instance.slotPath);
        currentSlot = Instance.slots[Instance.slotIndex.data];
    }
    private void ChangeSlot(SlotSO slot) {
        currentSlot = slot;
    }
}
