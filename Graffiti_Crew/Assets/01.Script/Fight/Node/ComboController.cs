using DG.Tweening;
using TMPro;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    private int _currentCombo = 0;

    private TextMeshProUGUI _comboText;
    private TextMeshProUGUI _stateText;

    private Vector3 _comboTextOrigin;
    private Sequence _comboTextSequence;
    private Sequence _stateTextSequence;

    private NodeJudgement _judgement;
    private StageResultSO _stageResult;

    private void Awake()
    {
        Transform parent = transform.Find("Panel_Combo");
        _comboText = parent.Find("Text_Combo").GetComponent<TextMeshProUGUI>();
        _stateText = parent.Find("Text_State").GetComponent<TextMeshProUGUI>();
        _comboTextOrigin = _comboText.rectTransform.anchoredPosition;
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

    public void SuccessCombo()
    {
        StateTextUpdate("<color=white>Combo</color>");
        ComboTextUpdate(++_currentCombo);
    }

    public void FailCombo()
    {
        _stageResult.comboCnt += _currentCombo;
        _currentCombo = 0;

        StateTextUpdate("<color=red>Miss</color>");
        ComboTextUpdate(_currentCombo);
    }

    private void ComboTextUpdate(int combo)
    {
        if (_comboText == null) return;

        _comboText.text = combo.ToString();

        // Init
        _comboText.DOFade(1f, 0f); 
        _comboText.rectTransform.anchoredPosition = 
            new Vector3(_comboTextOrigin.x, _comboTextOrigin.y - 10, _comboTextOrigin.z);

        _comboTextSequence.Complete();
        _comboTextSequence = DOTween.Sequence();
        _comboTextSequence.Append(_comboText.rectTransform.DOAnchorPosY(_comboTextOrigin.y, 0.2f).SetEase(Ease.OutBack))
                .AppendInterval(1f)
                .Append(_comboText.DOFade(0f, 0.3f));
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
            .Append(_stateText.transform.DOScale(1f, 0.1f))
                .AppendInterval(1f)
                .Append(_stateText.DOFade(0f, 0.3f));
    }
}
