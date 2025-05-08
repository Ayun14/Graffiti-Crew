using AH.UI.Events;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightSceneUIController : Observer<GameStateController>
{
    [Header("Other Panel")]
    [SerializeField] private Image _comboPanel;

    [Header("Cursor")]
    [SerializeField] private Texture2D _cursorTex;

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

    [Header("Interaction")]
    [SerializeField] private Image _interactionImage;

    // StepValue Sin Graph
    private float _frequency = 0.1f; // 주기
    private float _amplitude = 0.005f; // 변화 폭
    private float _baseStepValue;
    private float _elapsedTime = 0f;

    // Blind Shader
    private int _blindColor = Shader.PropertyToID("_BlindColor"); // 색상
    private int _stepValue = Shader.PropertyToID("_StepValue"); // noise에서 보이는 양 조절 0.44
    private int _lengthPower = Shader.PropertyToID("_LengthPower"); // 보이는 알파 0.77
    private int _offsetVec = Shader.PropertyToID("_OffsetVec"); // 보이는 uv 위치

    // Fail Shader
    private Image _failFeedbackPanel;
    private Material _failMat;
    private int _failPower = Shader.PropertyToID("_Power");

    // Loading
    private Image _loadingPanel;

    // Finish
    private Image _finishPanel;
    private Image _finishImage;

    // Transition
    private Image _transitionPanel;
    private Material _transitionMat;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;
        mySubject.OnRivalCheckEvent += RivalCheckEventHandle;
        mySubject.OnNodeFailEvent += NodeFailEventHandle;

        // Cursor
        Cursor.SetCursor(_cursorTex, Vector2.zero, CursorMode.Auto);

        Transform canvas = transform.Find("Canvas");

        // Blind Shader
        _blindPanel = canvas.Find("Panel_Blind").GetComponent<Image>();
        _foodImage = _blindPanel.transform.Find("Button_Food").GetComponent<FoodImage>();
        _blindMat = _blindPanel.transform.Find("Image_BlindShader").GetComponent<Image>().material;
        _blindMat.SetFloat(_stepValue, 0f);

        // Fail Shader
        _failFeedbackPanel = canvas.Find("Panel_FailFeedback").GetComponent<Image>();
        _failMat = _failFeedbackPanel.transform.Find("Image_FailShader").GetComponent<Image>().material;

        // Loding
        _loadingPanel = canvas.Find("Panel_Loading").GetComponent<Image>();

        // Finish
        _finishPanel = canvas.Find("Panel_Finish").GetComponent<Image>();
        _finishImage = _finishPanel.transform.Find("Image_Finish").GetComponent<Image>();

        // Transition
        _transitionPanel = canvas.Find("Panel_Transition").GetComponent<Image>();
        _transitionMat = _transitionPanel.transform.Find("Image_TransitionShader").GetComponent<Image>().material;
    }

    private void OnDestroy()
    {
        mySubject.OnBlindEvent -= BlindEventHandle;
        mySubject.OnRivalCheckEvent -= RivalCheckEventHandle;
        mySubject.OnNodeFailEvent -= NodeFailEventHandle;

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
            bool isFight = mySubject.GameState == GameState.Fight
                || mySubject.GameState == GameState.Tutorial;
            bool isFinish = mySubject.GameState == GameState.Finish;

            // Loding
            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            // Fight
            StageEvent.SetActiveFightViewEvent?.Invoke(isFight);
            _comboPanel.gameObject.SetActive(isFight);
            _failFeedbackPanel.gameObject.SetActive(isFight);

            if (isFinish && isBlind)
            {
                StopAllCoroutines();
                StartCoroutine(OffBlindRoutine());
            }
            else _blindPanel.gameObject.SetActive(isFight);

            // Finish
            if (isFinish)
            {
                StartCoroutine(FinishRoutine());
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Clock);
            }
            _finishPanel.gameObject.SetActive(isFinish);
        }
    }

    private IEnumerator FinishRoutine()
    {
        _finishImage.rectTransform.anchoredPosition = new Vector2(1400, 0);
        _finishImage.rectTransform.DOAnchorPosX(0, 1f).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(3f);
        _finishImage.rectTransform.DOAnchorPosX(-1400, 1f).SetEase(Ease.InOutBack);
    }

    #region Rival Check

    private void RivalCheckEventHandle()
    {
        UIAnimationEvent.SetActiveRivalCheckAnimationEvnet?.Invoke(true);
    }

    #endregion

    #region Blind Shader

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

    #region Fail Shader

    private void NodeFailEventHandle()
    {
        StartCoroutine(ChangeFailPower());
    }

    private IEnumerator ChangeFailPower()
    {
        float startValue = 15f;
        float endValue = 2f;

        yield return StartCoroutine(LerpFailPower(startValue, endValue, 0.3f));
        yield return StartCoroutine(LerpFailPower(endValue, startValue, 0.6f));
    }

    private IEnumerator LerpFailPower(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _failMat.SetFloat(_failPower, Mathf.Lerp(start, end, elapsed / duration));
            yield return null;
        }
        _failMat.SetFloat(_failPower, end);
    }

    #endregion

    #region Transition Shader

    public void StartTransitionRoutine() => StartCoroutine(TransitionRoutine());

    private IEnumerator TransitionRoutine()
    {
        _transitionPanel.gameObject.SetActive(true);
        _transitionMat.SetFloat("_Lerp", 0f);
        _transitionMat.DOFloat(0.2f, "_Lerp", 1.5f);
        yield return new WaitForSeconds(2f);

        _transitionMat.DOFloat(0f, "_Lerp", 0.3f);
        yield return new WaitForSeconds(1.2f);

        _transitionMat.DOFloat(0.2f, "_Lerp", 1.5f);
        yield return new WaitForSeconds(2f);

        _transitionMat.DOFloat(1f, "_Lerp", 0.3f);
        yield return new WaitForSeconds(0.8f);
    }

    #endregion

    #region Interaction Icon

    public void InteractionIcon()
    {
        _interactionImage.transform.localScale = Vector3.zero;
        _interactionImage.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce);
    }

    #endregion

    public void SetResultUI()
    {
        StageEvent.ShowResultViewEvent?.Invoke(true, mySubject.IsPlayerWin);
    }
}
