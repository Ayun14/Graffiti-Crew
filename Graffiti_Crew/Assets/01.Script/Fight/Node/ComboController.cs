using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    [SerializeField] private UIParticle _uiParticle;

    private int _currentCombo = 0;

    private TextMeshProUGUI _comboText;
    private TextMeshProUGUI _stateText;

    private Vector3 _comboTextOrigin;
    private Sequence _stateTextSequence;
    private Vector3 _stateTextOrigin;
    private float _textHideX = 120f;

    private NodeJudgement _judgement;
    private StageResultSO _stageResult;

    private bool _isCombo = false;
    private float _textHideTime = 2f;
    private float _currentTime;

    private void Awake()
    {
        Transform parent = transform.Find("Panel_Combo");
        _comboText = parent.Find("Text_Combo").GetComponent<TextMeshProUGUI>();
        _stateText = parent.Find("Text_State").GetComponent<TextMeshProUGUI>();
        _comboTextOrigin = _comboText.rectTransform.anchoredPosition;
        _stateTextOrigin = _stateText.rectTransform.anchoredPosition;
    }

    public void Init(NodeJudgement judgement)
    {
        _judgement = judgement;
        _stageResult = judgement.stageResult;
        _currentCombo = 0;

        if (_comboText != null && _stateText != null)
        {
            _comboText.text = string.Empty;
            _stateText.text = string.Empty;
        }
    }

    private void Update()
    {
        if (_isCombo)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _textHideTime)
            {
                _currentTime = 0;
                _isCombo = false;
                TextHide();
            }
        }
    }

    public void SuccessCombo()
    {
        if (_isCombo == false)
        {
            TextReset();
            _isCombo = true;
        }

        _currentTime = 0;
        StateTextUpdate("<color=white>Combo</color>");
        ComboTextUpdate(++_currentCombo);

        if (_currentCombo % 100 == 0)
        {
            // Particle
            //_uiParticle.Play();

            StartCoroutine(ComboEffectRoutine());
        }

        if (_currentCombo > _stageResult.comboCnt)
            _stageResult.comboCnt = _currentCombo;
    }

    public void FailCombo()
    {
        _isCombo = false;
        _currentCombo = 0;

        // Particle Stop
        _uiParticle.Stop();

        StateTextUpdate("<color=red>Miss</color>");
        ComboTextUpdate(_currentCombo);
    }

    private void ComboTextUpdate(int combo)
    {
        if (_comboText == null) return;

        _comboText.text = combo.ToString();

        // Init
        _comboText.DOFade(1f, 0f);
    }

    private void StateTextUpdate(string str)
    {
        if (_stateText == null) return;

        _stateText.text = str;

        // Init
        _stateText.DOFade(1f, 0f);
        _stateText.transform.localScale = Vector3.one;

        _stateTextSequence.Complete();
        _stateTextSequence = DOTween.Sequence();
        _stateTextSequence.Append(_stateText.transform.DOScale(1.1f, 0.1f))
            .Append(_stateText.transform.DOScale(1f, 0.1f));
    }

    private void TextReset()
    {
        _comboText.rectTransform.anchoredPosition = _comboTextOrigin;
        _stateText.rectTransform.anchoredPosition = _stateTextOrigin;
        _comboText.DOFade(1f, 0f);
        _stateText.DOFade(1f, 0f);
    }

    private void TextHide()
    {
        _comboText.rectTransform.DOAnchorPosX(_textHideX, 0.5f);
        _stateText.rectTransform.DOAnchorPosX(_textHideX, 0.5f);
        _comboText.DOFade(0f, 0.3f);
        _stateText.DOFade(0f, 0.3f);
    }

    private IEnumerator ComboEffectRoutine()
    {
        Vector3 endScale = new Vector3(1.7f, 1.7f, 1.7f);
        _comboText.transform.DOScale(endScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _comboText.transform.DOScale(Vector3.one, 0.3f);
    }
}
