using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivitySceneCharacterController : Observer<GameStateController>, INeedLoding
{
    [Header("Player")]
    [SerializeField] private GameObject _sprayCan;

    private Transform _playerTrm;
    private List<Transform> _rivalTrmList = new();

    private Transform _escapeTrm;
    private Transform _ellaGraffitiTrm;
    private List<Transform> _characterSpawnTrmList;

    private int _ellaSpawnIdx;

    private void Awake()
    {
        Attach();

        _playerTrm = transform.Find("PlayerAnim").GetComponent<Transform>();
        _escapeTrm = transform.Find("EscapePos").GetComponent<Transform>();
        _ellaGraffitiTrm = transform.Find("EllaGraffitiPos").GetComponent<Transform>();
        _characterSpawnTrmList = transform.Find("CharacterSpawnPos").GetComponentsInChildren<Transform>().Skip(1).ToList();
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
        if (mySubject.IsPlayerWin)
        {
            _playerTrm.position = _characterSpawnTrmList[0].localPosition;
            _playerTrm.rotation = _characterSpawnTrmList[0].localRotation;
        }


        for (int i = 0; i < _rivalTrmList.Count; i++)
        {
            // Ella
            if (_rivalTrmList[i].name == "EllaAnim_SprayCan(Clone)")
            {
                _rivalTrmList[i].position = _ellaGraffitiTrm.localPosition;
                _rivalTrmList[i].rotation = _ellaGraffitiTrm.localRotation;
            }
            // Others
            else
            {
                Transform trm = _characterSpawnTrmList[i + 1];
                _rivalTrmList[i].position = trm.localPosition;
                _rivalTrmList[i].rotation = trm.localRotation;
            }
        }
    }

    private void RivalsSpawn(List<Transform> rivalPrefabList)
    {
        _rivalTrmList?.Clear();
        for (int i = 0; i < rivalPrefabList.Count; i++)
        {
            Transform rivalSpawnTrm = _characterSpawnTrmList[i + 1];
            Transform rivalPrefab = rivalPrefabList[i];
            Transform trm = Instantiate(rivalPrefab.gameObject, rivalSpawnTrm.localPosition, rivalSpawnTrm.localRotation, transform).transform;
            if (trm.name == "EllaAnim_SprayCan(Clone)") _ellaSpawnIdx = i + 1;
            _rivalTrmList.Add(trm);
        }
    }

    #region Timeline

    public void CharacterFadeIn()
    {
        GameManager.Instance.CharacterFade(0f, 1f);
    }

    public void CharacterFadeOut()
    {
        GameManager.Instance.CharacterFade(1f, 1f);
    }

    public void CharacterWalkAnimation()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Walk);
        AnimationEvent.SetAnimation?.Invoke(4, AnimationEnum.Walk);
        AnimationEvent.SetAnimation?.Invoke(5, AnimationEnum.Walk);
    }

    public void RivalStartGraffiti()
    {
        for (int i = 0; i < _rivalTrmList.Count; ++i)
        {
            // Ella
            Debug.Log(_rivalTrmList[i].name);
            if (_rivalTrmList[i].name == "EllaAnim_SprayCan(Clone)")
            {
                _rivalTrmList[i].DORotate(new Vector3(0, 30f, 0), 0.3f);
                _rivalTrmList[i].DOMove(_ellaGraffitiTrm.localPosition, Random.Range(2f, 2.5f)).OnComplete(() =>
                {
                    AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
                });
            }
            // Others
            else
            {
                _rivalTrmList[i].DORotate(Vector3.zero, 0.3f);
                _rivalTrmList[i].DOMoveZ(-1f, Random.Range(1.5f, 2f)).OnComplete(() =>
                {
                    AnimationEvent.SetAnimation?.Invoke(4, AnimationEnum.Idle);
                    AnimationEvent.SetAnimation?.Invoke(5, AnimationEnum.Idle);
                });
            }
        }
    }

    public void EllaGoBack()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Walk);

        Transform ella = _rivalTrmList.Find(value => value.name == "EllaAnim_SprayCan(Clone)");
        ella.DORotate(new Vector3(0, -136, 0), 0.6f)
            .OnComplete(() =>
            {
                ella.DOMove(_characterSpawnTrmList[_ellaSpawnIdx].localPosition, 1.5f)
                .OnComplete(() =>
                {
                    ella.DORotateQuaternion(_characterSpawnTrmList[_ellaSpawnIdx].localRotation, 0.7f);
                    AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
                });
            });
    }

    public void RivalsEscape()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Run);
        AnimationEvent.SetAnimation?.Invoke(4, AnimationEnum.Run);
        AnimationEvent.SetAnimation?.Invoke(5, AnimationEnum.Run);
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
        AnimationEvent.SetAnimation?.Invoke(4, AnimationEnum.Talk);
        AnimationEvent.SetAnimation?.Invoke(5, AnimationEnum.Talk);
    }

    public void PlayerSprayNone()
    {
        AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.SprayNone);
        if (_sprayCan != null) _sprayCan.SetActive(true);
    }

    #endregion

    public void ResultTimelineSkip()
    {
        if (mySubject.IsPlayerWin)
        {
            _playerTrm.DOKill();
            _playerTrm.position = new Vector2(_escapeTrm.position.x - 10f, _playerTrm.position.y);
            foreach (Transform rivalTrm in _rivalTrmList)
            {
                rivalTrm.DOKill();
                rivalTrm.transform.position = _escapeTrm.position;
            }
        }
        else PlayerSprayNone();
    }
}
