using AH.LanguageSystem;
using AH.Save;
using AH.UI.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CursorType
{
    Normal,
    Spray,
    RMB, LMB
}

public class GameManager : MonoSingleton<GameManager>
{
    public static SlotSO currentSlot;
    private static bool _isPause = false;

    private string slotPath = "UI/Setting/Slots/";
    private SlotSO[] slots;
    public IntSaveDataSO slotIndex;

    [Header("Cursor")]
    [SerializeField] List<Texture2D> _cursorList;

    [Header("Character Material")]
    [SerializeField] private List<Material> _characterMatList = new();
    [SerializeField] private float _minValue = -2f, _maxValue = 2f;

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
        SetCursor(CursorType.Normal);
        GameManager.Instance.CharacterFade(1, 0);
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
        SaveDataEvents.ChangeSlotEvent?.Invoke();
    }

    #region Cursor

    public void SetCursor(CursorType cursorType)
    {
        int index = (int)cursorType;
        Cursor.SetCursor(_cursorList[index], Vector2.zero, CursorMode.Auto);
    }

    #endregion

    #region Character Fade

    public void CharacterFade(float targetValue, float duration)
    {
        bool isFadeIn = targetValue == 1f ? true : false;
        StartCoroutine(CharacterFadeRoutine(isFadeIn, duration));
    }

    private IEnumerator CharacterFadeRoutine(bool isFadeIn, float duration)
    {
        float elapsed = 0;
        float startValue = _characterMatList[0].GetFloat("_MinFadDistance");
        float targetValue = isFadeIn ? _maxValue : _minValue;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(startValue, targetValue, t);

            foreach (Material mat in _characterMatList)
                mat.SetFloat("_MinFadDistance", value);

            yield return null;
        }

        foreach (Material mat in _characterMatList)
            mat.SetFloat("_MinFadDistance", targetValue);
    }

    #endregion
}
