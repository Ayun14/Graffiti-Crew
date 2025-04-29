using AH.SaveSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DataController : Observer<GameStateController>
{
    [SerializeField] protected LoadStageSO stageSO;
    [HideInInspector] public StageDataSO stageData;

    private int _lodingCnt = 0;
    private List<GameObject> _needLodingObjs;

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Loding)
            {
                FindDatas();
                GiveData();
            }

            NotifyHandleChild();
        }
    }

    protected abstract void FindDatas();

    private void GiveData()
    {
        if (stageData != null)
        {
            _lodingCnt = 0;
            if (_needLodingObjs != null) _needLodingObjs.Clear();
            _needLodingObjs = GameObject.FindGameObjectsWithTag("NeedLoding").ToList();

            foreach (GameObject obj in _needLodingObjs)
            {
                if (obj.TryGetComponent(out INeedLoding needLoding))
                    needLoding.LodingHandle(this);
            }
        }
    }

    public void SuccessGiveData()
    {
        if (++_lodingCnt >= _needLodingObjs.Count)
            Invoke("FinishGiveData", 0.5f);
    }
            
    protected abstract void FinishGiveData(); // Change GameState...

    protected abstract void NotifyHandleChild();
}
