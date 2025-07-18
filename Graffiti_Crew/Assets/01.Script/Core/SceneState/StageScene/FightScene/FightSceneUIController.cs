using AH.UI.Events;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FightSceneUIController : Observer<GameStateController>
{
    [Header("Blind")]
    [SerializeField] private List<Sprite> _inkSprites = new();
    private Image _inkImage1;
    private Image _inkImage2;

    [SerializeField] private Sprite _eggSprite;
    [SerializeField] private Sprite _tomatoSprite;
    [SerializeField] private float _blindTime;
    [SerializeField] private float _fastBlindPercent = 0.1f; // 음식 눌렀을 때 줄일 알파의 퍼센트
    private float _currentBlindTime;
    private Image _blindPanel;
    private FoodImage _foodImage;
    public bool isBlind => mySubject.IsBlind;

    [Header("Interaction")]
    [SerializeField] private Image _interactionImage;

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

    // Timeline
    private Image _timelinePanel;

    private void Awake()
    {
        Attach();

        mySubject.OnBlindEvent += BlindEventHandle;
        mySubject.OnRivalCheckEvent += RivalCheckEventHandle;
        mySubject.OnNodeFailEvent += NodeFailEventHandle;

        Transform stageCanvas = transform.Find("Canvas_Stage");

        // Blind Shader
        _blindPanel = stageCanvas.Find("Panel_Blind").GetComponent<Image>();
        _inkImage1 = _blindPanel.transform.Find("Image_Ink1").GetComponent<Image>();
        _inkImage1.sprite = null;
        _inkImage2 = _blindPanel.transform.Find("Image_Ink2").GetComponent<Image>();
        _inkImage2.sprite = null;

        // Fail Shader
        _failFeedbackPanel = stageCanvas.Find("Panel_FailFeedback").GetComponent<Image>();
        _failMat = _failFeedbackPanel.transform.Find("Image_FailShader").GetComponent<Image>().material;

        // Loding
        _loadingPanel = stageCanvas.Find("Panel_Loading").GetComponent<Image>();

        // Finish
        _finishPanel = stageCanvas.Find("Panel_Finish").GetComponent<Image>();
        _finishImage = _finishPanel.transform.Find("Image_Finish").GetComponent<Image>();

        // Transition
        _transitionPanel = stageCanvas.Find("Panel_Transition").GetComponent<Image>();
        _transitionMat = _transitionPanel.transform.Find("Image_TransitionShader").GetComponent<Image>().material;

        // Timeline
        _timelinePanel = transform.Find("Canvas_Timeline").Find("Panel_Timeline").GetComponent<Image>();
    }

    private void OnDestroy()
    {
        mySubject.OnBlindEvent -= BlindEventHandle;
        mySubject.OnRivalCheckEvent -= RivalCheckEventHandle;
        mySubject.OnNodeFailEvent -= NodeFailEventHandle;

        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            bool isFight = mySubject.GameState == GameState.Fight
                || mySubject.GameState == GameState.Tutorial;
            bool isFinish = mySubject.GameState == GameState.Finish;

            // Timeline
            bool isTimelineText = mySubject.GameState == GameState.Timeline || mySubject.GameState == GameState.Result;
            //bool isTimelineText = mySubject.GameState == GameState.Timeline;
            _timelinePanel.gameObject.SetActive(isTimelineText);

            // Loding
            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            // Fight
            _failFeedbackPanel.gameObject.SetActive(isFight);

            if (isFight) {
                // Fight UI
                StageEvent.SetActiveFightViewEvent?.Invoke(true);

                // Cursor
                GameManager.Instance.SetCursor(CursorType.Spray);
            }

            if (isFinish && isBlind)
            {
                StopAllCoroutines();
                //StartCoroutine(OffBlindRoutine());
                SetBlindAlpha(0, 0.3f, new List<Image> { _inkImage1, _inkImage2 }, () => mySubject.SetIsBlind(false));
            }
            else _blindPanel.gameObject.SetActive(isFight);

            // Finish
            if (isFinish) {
                // Fight UI
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                // Cursor
                GameManager.Instance.SetCursor(CursorType.Normal);

                StartCoroutine(FinishRoutine());
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Clock);
                GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_Mouse);
                GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Click_UI);
            }
            _finishPanel.gameObject.SetActive(isFinish);

            if (mySubject.GameState == GameState.Result)
            {
                UIAnimationEvent.SetFilmDirectingEvent(true);
            }

            if (mySubject.GameState == GameState.NextStage)
            {
                StageEvent.ShowResultViewEvent?.Invoke(true, mySubject.IsPlayerWin);
            }
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

    #region Blind

    private void BlindEventHandle()
    {
        //StartCoroutine(OnBlindRoutine());
        StartCoroutine(StartBlindRoutine());
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

    private IEnumerator StartBlindRoutine()
    {
        mySubject.SetIsBlind(true);

        SetInkImage(_inkImage1);
        yield return new WaitForSeconds(0.3f);
        SetInkImage(_inkImage2);

        yield return new WaitForSeconds(_blindTime);

        SetBlindAlpha(0, 0.3f, new List<Image> { _inkImage1, _inkImage2 });

        mySubject.SetIsBlind(false);
    }

    private void SetInkImage(Image image)
    {
        image.sprite = _inkSprites[Random.Range(0, _inkSprites.Count)];
        SetBlindAlpha(1f, 0.2f, image);

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Short);
    }

    private void SetBlindAlpha(float alpha, float time, Image image, Action callback = null)
    {
        image.DOFade(alpha, time).OnComplete(()=> callback?.Invoke());
    }

    private void SetBlindAlpha(float alpha, float time, List<Image> images, Action callback = null)
    {
        foreach (Image image in images)
            image.DOFade(alpha, time).OnComplete(() => callback?.Invoke());
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
        _transitionMat.SetFloat("_Lerp", 1f);
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
