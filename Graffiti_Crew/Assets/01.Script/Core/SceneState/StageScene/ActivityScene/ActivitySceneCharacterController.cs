using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivitySceneCharacterController : Observer<GameStateController>, INeedLoding
{
    [Header("Fade")]
    [SerializeField] private List<Material> _characterMatList = new();
    [Range(-5f, 5f)]
    [SerializeField] private float _minValue = -2f, _maxValue = 2f;

    private Transform _playerTrm;
    private List<Transform> _rivalTrmList = new();

    private Transform _escapeTrm;
    private Transform _ellaGraffitiTrm;
    private List<Transform> _characterSpawnTrmList;

    private void Awake()
    {
        Attach();

        _playerTrm = transform.Find("PlayerAnim").GetComponent<Transform>();
        _escapeTrm = transform.Find("EscapePos").GetComponent<Transform>();
        _ellaGraffitiTrm = transform.Find("EllaGraffitiPos").GetComponent<Transform>();
        _characterSpawnTrmList = transform.Find("CharacterSpawnPos").GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    // Test Code
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    foreach (Material mat in _characterMatList)
        //        mat.SetFloat("_MinFadDistance", _minValue);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    foreach (Material mat in _characterMatList)
        //        mat.SetFloat("_MinFadDistance", _maxValue);
        //}
    }

    private void OnDestroy()
    {
        Detach();
    }

    #region Handles
    public void LodingHandle(DataController dataController)
    {
        RivalsSpawn(dataController.stageData.rivalPrefabList);
        dataController.SuccessGiveData();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Finish)
            {
                CharacterFadeOut();
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
                SettingFinishPos();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                _playerTrm.DOMoveY(0f, 1f);
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Paint);
            }
        }
    }
    #endregion

    private void SettingFinishPos()
    {
        // Player
        _playerTrm.position = _characterSpawnTrmList[0].localPosition;
        _playerTrm.rotation = _characterSpawnTrmList[0].localRotation;

        // Ella
        _rivalTrmList[0].position = _ellaGraffitiTrm.localPosition;
        _rivalTrmList[0].rotation = _ellaGraffitiTrm.localRotation;

        // Others
        for (int i = 1; i < _rivalTrmList.Count; i++)
        {
            Transform trm = _characterSpawnTrmList[i + 1];
            _rivalTrmList[i].position = trm.localPosition;
            _rivalTrmList[i].rotation = trm.localRotation;
        }
    }

    #region Fade

    public void CharacterFadeIn()
    {
        StartCoroutine(CharacterFadeRoutine(true, 1f));
    }

    public void CharacterFadeOut()
    {
        StartCoroutine(CharacterFadeRoutine(false, 1f));
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

    private void RivalsSpawn(List<Transform> rivalPrefabList)
    {
        _rivalTrmList?.Clear();
        for (int i = 0; i < rivalPrefabList.Count; i++)
        {
            Transform rivalSpawnTrm = _characterSpawnTrmList[i + 1];
            Transform rivalPrefab = rivalPrefabList[i];
            Transform trm = Instantiate(rivalPrefab.gameObject, rivalSpawnTrm.localPosition, rivalSpawnTrm.localRotation, transform).transform;
            _rivalTrmList.Add(trm);
        }
    }

    #region Timeline

    public void RivalStartGraffiti()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Walk);

        // Others
        for (int i = 1; i < _rivalTrmList.Count; ++i)
        {
            _rivalTrmList[i].DORotate(Vector3.zero, 0.3f);
            _rivalTrmList[i].DOMoveZ(-1f, Random.Range(1.5f, 2f)).OnComplete(() =>
            {
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
            });
        }

        // Ella
        _rivalTrmList[0].DORotate(new Vector3(0, 30f, 0), 0.3f);
        _rivalTrmList[0].DOMove(_ellaGraffitiTrm.localPosition, Random.Range(2f, 2.5f)).OnComplete(() =>
        {
            AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
        });
    }

    public void EllaGoBack()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Walk);

        Transform ella = _rivalTrmList[0];
        ella.DORotate(new Vector3(0, -136, 0), 0.6f)
            .OnComplete(() =>
            {
                ella.DOMove(_characterSpawnTrmList[1].localPosition, 2f)
                .OnComplete(() =>
                {
                    ella.DORotateQuaternion(_characterSpawnTrmList[1].localRotation, 0.7f);
                    AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
                });
            });
    }

    public void RivalsEscape()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Run);
        foreach (Transform rivalTrm in _rivalTrmList)
        {
            rivalTrm.DORotateQuaternion(_escapeTrm.localRotation, 0.5f);
            rivalTrm.DOMoveX(_escapeTrm.position.x, Random.Range(4f, 6f));
        }
    }

    public void RivalsGone()
    {
        foreach (Transform rivalTrm in _rivalTrmList)
            rivalTrm.position = _escapeTrm.position;
    }

    public void RivalsTalk()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Talk);
    }

    #endregion
}
