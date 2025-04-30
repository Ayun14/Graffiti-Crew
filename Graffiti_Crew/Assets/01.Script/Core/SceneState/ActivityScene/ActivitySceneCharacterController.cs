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
        Debug.Log(dataController.stageData.rivalPrefabList.Count);
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
        foreach(Transform rivalSpawnTrm in _rivalSpawnTrmList)
        {
            if (rivalPrefabList.Count == 0) break;

            Debug.Log(rivalSpawnTrm.position);
            Transform trm = Instantiate(rivalPrefabList.First().gameObject, rivalSpawnTrm.position, rivalSpawnTrm.localRotation, transform).transform;
            _rivalTrmList.Add(trm);
            rivalPrefabList.Remove(rivalPrefabList.First());
        }
    }

    public void RivalsEscape()
    {
        foreach(Transform rivalTrm in _rivalTrmList)
        {
            // 나머지 처리 하기 Animation & Rotation
            rivalTrm.DOMoveX(_escapeTrm.position.x, Random.Range(4f, 6f));
        }
    }
}
