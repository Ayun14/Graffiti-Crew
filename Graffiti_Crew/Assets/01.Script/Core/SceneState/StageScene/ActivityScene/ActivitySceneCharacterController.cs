using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ActivitySceneCharacterController : Observer<GameStateController>, INeedLoding
{
    private Transform _playerTrm;
    private List<Transform> _rivalTrmList = new();

    private Transform _escapeTrm;
    private List<Transform> _rivalSpawnTrmList;

    private void Awake()
    {
        _playerTrm = transform.Find("PlayerAnim").GetComponent<Transform>();
        _escapeTrm = transform.Find("EscapePos").GetComponent<Transform>(); 
        _rivalSpawnTrmList = transform.Find("RivalSpawnPos").GetComponentsInChildren<Transform>().Skip(1).ToList();
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

        }
    }

    #endregion

    private void RivalsSpawn(List<Transform> rivalPrefabList)
    {
        _rivalTrmList?.Clear();
        for (int i = 0; i < rivalPrefabList.Count; i++)
        {
            Transform rivalSpawnTrm = _rivalSpawnTrmList[i];
            Transform rivalPrefab = rivalPrefabList[i];
            Transform trm = Instantiate(rivalPrefab.gameObject, rivalSpawnTrm.position, rivalSpawnTrm.localRotation, transform).transform;
            _rivalTrmList.Add(trm);
        }
    }

    public void RivalsEscape()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Run);
        foreach (Transform rivalTrm in _rivalTrmList)
        {
            // 나머지 처리 하기 Animation & Rotation
            rivalTrm.DORotateQuaternion(_escapeTrm.localRotation, 0.5f);
            rivalTrm.DOMoveX(_escapeTrm.position.x, Random.Range(4f, 6f));
        }
    }

    public void SetRivalAnimation(AnimationEnum animation)
    {
        AnimationEvent.SetAnimation(2, animation);
    }
}
