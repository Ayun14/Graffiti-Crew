using AH.LanguageSystem;
using AH.SaveSystem;
using AH.UI.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public static SlotSO currentSlot;
    private static bool _isPause = false;

    private string slotPath = "UI/Setting/Slots/";
    private SlotSO[] slots;
    public IntSaveDataSO slotIndex;

    [Header("Cursor")]
    [SerializeField] private Texture2D _normalCursor;
    [SerializeField] private Texture2D _sprayCursor;

    // 해상도 설정
    private int width = 1920;
    private int height = 1080;

    #region Systems

    public SoundManager SoundSystemCompo { get; private set; }
    public LanguageSystem LanguageSystemCompo { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        SoundSystemCompo = GetComponent<SoundManager>();
        LanguageSystemCompo = GetComponent<LanguageSystem>();
    }

    private void OnEnable()
    {
        UIEvents.ChangeSlotEvent += ChangeSlot;
        SceneManager.sceneLoaded += SetResolution;
    }

    private void OnDisable()
    {
        UIEvents.ChangeSlotEvent -= ChangeSlot;
        SceneManager.sceneLoaded -= SetResolution;
    }

    private void SetResolution(Scene arg0, LoadSceneMode arg1)
    {
        Screen.SetResolution(width, height, true);
        SetCursor(false);
    }

    public static void SetPause(bool pause)
    { // true : stop
        _isPause = pause;
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public static bool IsPause()
    {
        return _isPause;
    }
    public static void SetSlot()
    {
        Instance.slots = Resources.LoadAll<SlotSO>(Instance.slotPath);
        currentSlot = Instance.slots[Instance.slotIndex.data];
    }
    private void ChangeSlot(SlotSO slot)
    {
        currentSlot = slot;
    }

    #region Cursor

    public void SetCursor(bool isSprayCursor)
    {
        Texture2D cursor = isSprayCursor ? _sprayCursor : _normalCursor;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    #endregion
}
