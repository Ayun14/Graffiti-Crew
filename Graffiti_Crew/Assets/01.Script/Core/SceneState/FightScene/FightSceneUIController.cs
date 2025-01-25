using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class FightSceneUIController : Observer<GameStateController>
{
    [SerializeField] private Image _spraySliderPanel;

    // Shader
    private Image _blindPanel;
    private Material _blindMat;
    private int _stepValue = Shader.PropertyToID("_StepValue"); // noise에서 보이는 양 조절 0.44
    private int _lengthPower = Shader.PropertyToID("_LengthPower"); // 보이는 알파 0.77
    private int _noiseValue = Shader.PropertyToID("_NoiseValue"); // 보이는 noise 크기 15
    private int _offsetVec = Shader.PropertyToID("_OffsetVec"); // 보이는 uv 위치

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

        Transform canvas = transform.Find("Canvas");

        // Shader
        _blindPanel = canvas.transform.Find("Panel_Blind").GetComponent<Image>();
        _blindMat = _blindPanel.transform.Find("Image_Blind").GetComponent<Image>().material;
        _blindMat.SetFloat(_stepValue, 0f);

        // CountDown
        _countDownPanel = canvas.transform.Find("Panel_CountDown").GetComponent<Image>();
        _countDownText = _countDownPanel.transform.Find("Text_CountDown").GetComponent<TextMeshProUGUI>();

        // Finish
        _finishPanel = canvas.transform.Find("Panel_Finish").GetComponent<Image>();
        _finishText = _finishPanel.transform.Find("Text_Finish").GetComponent<TextMeshProUGUI>();

        // Result
        _resultPanel = canvas.transform.Find("Panel_Result").GetComponent<Image>();
        _resultText = _resultPanel.transform.Find("Text_Result").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(OnBlindRoutine());
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(OffBlindRoutine());
        }
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            // CountDown
            bool isCountDown = mySubject.GameState == GameState.CountDown;
            if (isCountDown) StartCoroutine(CountDownRoutine());
            _countDownPanel.gameObject.SetActive(isCountDown);
            
            // Fight
            _spraySliderPanel.gameObject.SetActive(mySubject.GameState == GameState.Fight);
            _blindPanel.gameObject.SetActive(mySubject.GameState == GameState.Fight);

            // Finish
            _finishPanel.gameObject.SetActive(mySubject.GameState == GameState.Finish);

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

    private IEnumerator OnBlindRoutine()
    {
        //float randomNoise = Random.Range(14f, 20f);
        //_blindMat.SetFloat(_noiseValue, randomNoise);

        Vector2 randomOffset = new Vector2(Random.Range(0f, 20f), Random.Range(0f, 20f));
        _blindMat.SetVector(_offsetVec, randomOffset);

        float time = 1f;
        float currentTime = 0f;
        float targetStepValue = Random.Range(0.35f, 0.45f);
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
        float time = 1f;
        float currentTime = 0f;
        float currentStepValue = _blindMat.GetFloat(_stepValue);

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;

            float stepValue = Mathf.Lerp(currentStepValue, 0f, currentTime / time);
            _blindMat.SetFloat(_stepValue, stepValue);

            yield return null;
        }
    }
}
