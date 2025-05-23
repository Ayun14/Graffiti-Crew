using AH.LanguageSystem;
using AH.SaveSystem;
using AH.UI.Events;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {
    public static SlotSO currentSlot;
    private static bool _isPause = false;

    private string slotPath = "UI/Setting/Slots/";
    private SlotSO[] slots;
    public IntSaveDataSO slotIndex;

    #region Systems

    public SoundManager SoundSystemCompo { get; private set; }
    public LanguageSystem LanguageSystemCompo { get; private set; }

    #endregion

    protected override void Awake() {
        base.Awake();

        SoundSystemCompo = GetComponent<SoundManager>();
        LanguageSystemCompo = GetComponent<LanguageSystem>();
    }
    private void OnEnable() {
        UIEvents.ChangeSlotEvent += ChangeSlot;
    }
    private void OnDisable() {
        UIEvents.ChangeSlotEvent -= ChangeSlot;
    }
    public static void SetPause(bool pause) { // true : stop
        _isPause = pause;
        if (pause) {
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f;
        }
    }
    public static bool IsPause() {
        return _isPause;
    }
    public static void SetSlot() {
        Instance.slots = Resources.LoadAll<SlotSO>(Instance.slotPath);
        currentSlot = Instance.slots[Instance.slotIndex.data];
    }
    private void ChangeSlot(SlotSO slot) {
        currentSlot = slot;
    }
}
