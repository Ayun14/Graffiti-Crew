using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightSceneUIController : Observer<GameStateController>
{
    [Header("Other Panel")]
    [SerializeField] private Image _spraySliderPanel;
    [SerializeField] private Image _comboSliderPanel;

    [Header("Blind")]
    [SerializeField] private Sprite _eggSprite;
    [SerializeField] private Sprite _tomatoSprite;
    private Image _blindPanel;
    private FoodImage _foodImage;
    private Material _blindMat;
    private bool _isBlind = false;

    // StepValue Sin Graph
    private float _frequency = 0.1f; // 주기
    private float _amplitude = 0.005f; // 변화 폭
    private float _baseStepValue;
    private float _elapsedTime = 0f;

    // Shader
    private int _blindColor = Shader.PropertyToID("_BlindColor"); // 색상
    private int _stepValue = Shader.PropertyToID("_StepValue"); // noise에서 보이는 양 조절 0.44
    private int _lengthPower = Shader.PropertyToID("_LengthPower"); // 보이는 알파 0.77
    private int _noiseValue = Shader.PropertyToID("_NoiseValue"); // 보이는 noise 크기 15
    private int _offsetVec = Shader.PropertyToID("_OffsetVec"); // 보이는 uv 위치

    // Loding
    private Image _lodingPanel;

    // CountDown
    private Image _countDownPanel;
    private TextMeshProUGUI _countDownText;

    // Finish
    private Image _finishPanel;
    private TextMeshProUGUI _finishText;

    // Result
    private Image _resultPanel;
    private TextMeshProUGUI _resultText;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;
        mySubject.OnRivalCheckEvent += RivalCheckEventHandle;

        Transform canvas = transform.Find("Canvas");

        // Shader
        _blindPanel = canvas.Find("Panel_Blind").GetComponent<Image>();
        _foodImage = _blindPanel.transform.Find("Button_Food").GetComponent<FoodImage>();
        _blindMat = _blindPanel.transform.Find("Image_BlindShader").GetComponent<Image>().material;
        _blindMat.SetFloat(_stepValue, 0f);

        // Loding
        _lodingPanel = canvas.Find("Panel_Loding").GetComponent<Image>();

        // CountDown
        _countDownPanel = canvas.Find("Panel_CountDown").GetComponent<Image>();
        _countDownText = _countDownPanel.transform.Find("Text_CountDown").GetComponent<TextMeshProUGUI>();

        // Finish
        _finishPanel = canvas.Find("Panel_Finish").GetComponent<Image>();
        _finishText = _finishPanel.transform.Find("Text_Finish").GetComponent<TextMeshProUGUI>();

        // Result
        _resultPanel = canvas.Find("Panel_Result").GetComponent<Image>();
        _resultText = _resultPanel.transform.Find("Text_Result").GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        mySubject.OnBlindEvent -= BlindEventHandle;
        mySubject.OnRivalCheckEvent -= RivalCheckEventHandle;

        Detach();
    }

    private void Update()
    {
        ChangeBlindStepValue();
    }

    private void ChangeBlindStepValue()
    {
        if (_isBlind)
        {
            _elapsedTime += Time.deltaTime;

            float newValue = _baseStepValue + Mathf.Sin(_elapsedTime * _frequency * Mathf.PI * 2) * _amplitude;
            _blindMat.SetFloat(_stepValue, newValue);
        }
        else
            _elapsedTime = 0f;
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            bool isCountDown = mySubject.GameState == GameState.CountDown;
            bool isFight = mySubject.GameState == GameState.Fight;
            bool isFinish = mySubject.GameState == GameState.Finish;

            // Loding
            _lodingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            // CountDown
            if (isCountDown) StartCoroutine(CountDownRoutine());
            _countDownPanel.gameObject.SetActive(isCountDown);

            // Fight
            _spraySliderPanel.gameObject.SetActive(isFight);
            _comboSliderPanel.gameObject.SetActive(isFight);

            if (isFinish && _isBlind)
                StartBlindRoutine(false);
            else
                _blindPanel.gameObject.SetActive(isFight);

            // Finish
            _finishPanel.gameObject.SetActive(isFinish);

            // Result
            _resultPanel.gameObject.SetActive(mySubject.GameState == GameState.Result);
        }
    }

    private IEnumerator CountDownRoutine()
    {
        for (int i = 3; i > 0; --i)
        {
            _countDownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        mySubject.ChangeGameState(GameState.Fight);
    }

    #region Rival Check

    private void RivalCheckEventHandle()
    {
        // 여기에 UI 구현
    }

    #endregion

    #region Blind

    private void BlindEventHandle()
    {
        if (_isBlind) return;

        StartBlindRoutine(true);
    }

    public void StartBlindRoutine(bool isOn)
    {
        if (isOn) StartCoroutine(OnBlindRoutine());
        else StartCoroutine(OffBlindRoutine());
    }

    private IEnumerator OnBlindRoutine()
    {
        _isBlind = true;

        bool isEgg = Random.Range(0, 2) == 0 ? true : false;
        // Sprite
        Sprite sprite = isEgg ? _eggSprite : _tomatoSprite;
        _foodImage.OnFoodSprite(sprite);

        // Color
        Color blindColor = isEgg ? Color.yellow : Color.red;
        _blindMat.SetColor(_blindColor, blindColor);

        // Offset
        Vector2 randomOffset = new Vector2(Random.Range(0f, 20f), Random.Range(0f, 20f));
        _blindMat.SetVector(_offsetVec, randomOffset);

        // Step Value
        float time = 0.5f;
        float currentTime = 0f;
        float targetStepValue = Random.Range(0.45f, 0.51f);
        _baseStepValue = targetStepValue;
        _blindMat.SetFloat(_stepValue, 0);

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            float stepValue = Mathf.Lerp(0f, targetStepValue, currentTime / time);
            _blindMat.SetFloat(_stepValue, stepValue);

            yield return null;
        }
    }

    private IEnumerator OffBlindRoutine()
    {
        // Sprite
        _foodImage.OffFoodSprite();

        // Bool
        bool isFinish = mySubject.GameState == GameState.Finish;

        // Step Value
        float time = isFinish ? 0.5f : 1f;
        float currentTime = 0f;
        float currentStepValue = _blindMat.GetFloat(_stepValue);

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            float stepValue = Mathf.Lerp(currentStepValue, 0f, currentTime / time);
            _blindMat.SetFloat(_stepValue, stepValue);

            yield return null;
        }

        if (isFinish)
            _blindPanel.gameObject.SetActive(false);

        _isBlind = false;
    }

    #endregion
}
