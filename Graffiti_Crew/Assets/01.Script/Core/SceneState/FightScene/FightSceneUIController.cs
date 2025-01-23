using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightSceneUIController : Observer<GameStateController>
{
    [SerializeField] private Image _spraySliderImage;

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
            _spraySliderImage.gameObject.SetActive(mySubject.GameState == GameState.Fight);

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
}
