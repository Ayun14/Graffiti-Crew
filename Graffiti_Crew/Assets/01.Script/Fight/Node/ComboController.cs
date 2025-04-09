using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboController : MonoBehaviour
{
    [SerializeField] private UIParticle _uiParticle;

    [Header("Combo")]
    [SerializeField] private Sprite _comboSprite;
    [SerializeField] private Sprite _missSprite;

    private int _currentCombo = 0;

    private TextMeshProUGUI _comboText;
    private Image _stateImage;

    private Vector3 _comboTextOrigin;
    private Sequence _stateTextSequence;
    private Vector3 _stateImageOrigin;

    protected StageGameRule _stageGameRule;
    private StageResultSO _stageResult;

    private bool _isCombo = false;
    private float _textHideTime = 2f;
    private float _currentTime;

    private void Awake()
    {
        Transform parent = transform.Find("Panel_Combo");
        _comboText = parent.Find("Text_Combo").GetComponent<TextMeshProUGUI>();
        _stateImage = parent.Find("Image_State").GetComponent<Image>();
        _comboTextOrigin = _comboText.rectTransform.anchoredPosition;
        _stateImageOrigin = _stateImage.rectTransform.anchoredPosition;
    }

    public void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;
        _stageResult = stageGameRule.stageResult;
        _currentCombo = 0;

        if (_comboText != null && _stateImage != null)
        {
            _comboText.text = string.Empty;
            _stateImage.DOFade(0, 0f);
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
        StateTextUpdate(_comboSprite);
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
        //_uiParticle.Stop();

        StateTextUpdate(_missSprite);
        ComboTextUpdate(_currentCombo);
    }

    private void ComboTextUpdate(int combo)
    {
        if (_comboText == null) return;

        _comboText.text = combo.ToString();

        // Init
        _comboText.DOFade(1f, 0f);
    }

    private void StateTextUpdate(Sprite sprite)
    {
        if (_stateImage == null) return;

        if (_stateImage.sprite != sprite)
            _stateImage.sprite = sprite;

        // Init
        _stateImage.DOFade(1f, 0f);
        _stateImage.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        _stateTextSequence.Complete();
        _stateTextSequence = DOTween.Sequence();
        _stateTextSequence.Append(_stateImage.rectTransform.DOScale(0.42f, 0.1f))
            .Append(_stateImage.rectTransform.DOScale(0.4f, 0.1f));
    }

    private void TextReset()
    {
        _comboText.rectTransform.DOKill();
        _stateImage.rectTransform.DOKill();

        _comboText.rectTransform.anchoredPosition = _comboTextOrigin;



        _stateImage.rectTransform.anchoredPosition = _stateImageOrigin;
        _comboText.DOFade(1f, 0f);
        _stateImage.DOFade(1f, 0f);
    }

    private void TextHide()
    {
        _comboText.rectTransform.DOAnchorPosX(129.7f, 0.5f);
        _stateImage.rectTransform.DOAnchorPosX(218f, 0.5f);
        _comboText.DOFade(0f, 0.3f);
        _stateImage.DOFade(0f, 0.3f);
    }

    private IEnumerator ComboEffectRoutine()
    {
        Vector3 endScale = new Vector3(1.7f, 1.7f, 1.7f);
        _comboText.transform.DOScale(endScale, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _comboText.transform.DOScale(Vector3.one, 0.3f);
    }
}
