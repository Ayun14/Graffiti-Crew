using AH.UI.Events;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DG.Tweening.DOTweenAnimation;

public class FightSceneUIController : Observer<GameStateController>
{
    [Header("Other Panel")]
    [SerializeField] private Image _comboSliderPanel;

    [Header("Blind")]
    [SerializeField] private Sprite _eggSprite;
    [SerializeField] private Sprite _tomatoSprite;
    [SerializeField] private float _blindTime;
    [SerializeField] private float _fastBlindPercent = 0.1f; // 음식 눌렀을 때 줄일 알파의 퍼센트
    private float _currentBlindTime;
    private Image _blindPanel;
    private FoodImage _foodImage;
    private Material _blindMat;
    public bool isBlind => mySubject.IsBlind;

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
        if (isBlind)
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
            FightEvent.SetActiveFightViewEvent(isFight);
            _comboSliderPanel.gameObject.SetActive(isFight);

            if (isFinish && isBlind)
            {
                StopAllCoroutines();
                StartCoroutine(OffBlindRoutine());
            }
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

            // Position
            _countDownText.rectTransform.localPosition = new Vector3(0, -100f, 0);

            Sequence posSequence = DOTween.Sequence();
            posSequence.Append(_countDownText.rectTransform.DOAnchorPosY(0f, 0.3f))
                    .AppendInterval(0.4f)
                    .Append(_countDownText.rectTransform.DOAnchorPosY(100f, 0.3f));

            // Alpha
            Color color = _countDownText.color;
            color.a = 0f;
            _countDownText.color = color;

            Sequence alphaSequence = DOTween.Sequence();
            alphaSequence.Append(_countDownText.DOFade(1f, 0.3f))
                    .AppendInterval(0.4f)
                    .Append(_countDownText.DOFade(0f, 0.3f));


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
        StartCoroutine(OnBlindRoutine());
    }

    public void BlindFastEvent()
    {
        float targetTime = _blindTime * _fastBlindPercent + _currentBlindTime;
        
        // Length Power
        DOTween.To(() => _currentBlindTime,
        x => _currentBlindTime = x,
                   targetTime, 0.3f).SetEase(Ease.InOutQuad);

        // Sprite
        _foodImage.SetCurrentTime(targetTime);
    }

    private IEnumerator OnBlindRoutine()
    {
        mySubject.SetIsBlind(true);

        bool isEgg = Random.Range(0, 2) == 0 ? true : false;
        // Sprite
        Sprite sprite = isEgg ? _eggSprite : _tomatoSprite;
        _foodImage.OnFoodSprite(sprite);

        // Length Power
        _blindMat.SetFloat(_lengthPower, Random.Range(3.5f, 5f));

        // Color
        string blindColorString = isEgg ? "#ECE9E2" : "#E14722";
        if (ColorUtility.TryParseHtmlString(blindColorString, out Color blindColor))
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

        StartCoroutine(BlindRoutine());
    }

    private IEnumerator BlindRoutine()
    {
        // Sprite
        _foodImage.SetDoFade(_blindTime);

        // Length Power
        _currentBlindTime = 0f;
        float currentLengthPower = _blindMat.GetFloat(_lengthPower);

        while (_currentBlindTime < _blindTime)
        {
            _currentBlindTime += Time.deltaTime;

            float lengthPower = Mathf.Lerp(currentLengthPower, 0f, _currentBlindTime / _blindTime);
            _blindMat.SetFloat(_lengthPower, lengthPower);

            if (lengthPower <= 0f) break;

            yield return null;
        }

        StartCoroutine(OffBlindRoutine());
    }


    private IEnumerator OffBlindRoutine()
    {
        if (mySubject.GameState == GameState.Finish)
        {
            // Sprite
            float time = 0.5f;
            _foodImage.StopAllCoroutine();
            _foodImage.SetDoFade(0, time);

            // Length Power
            float currentTime = 0f;
            float currentLengthPower = _blindMat.GetFloat(_lengthPower);

            while (currentTime < time)
            {
                currentTime += Time.deltaTime;

                float lengthPower = Mathf.Lerp(currentLengthPower, 0f, currentTime / time);
                _blindMat.SetFloat(_lengthPower, lengthPower);

                yield return null;
            }

            _blindPanel.gameObject.SetActive(false);
        }

        mySubject.SetIsBlind(false);
    }

    #endregion
}
