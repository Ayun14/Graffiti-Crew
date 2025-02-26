using AH.Map;
using UnityEngine;

public abstract class DataController : Observer<GameStateController>
{
    [SerializeField] private LoadStageSO _stageSO;

    protected StageDataSO _stageData;

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

    private void FindDatas()
    {
        _stageData = Resources.Load("StageData/" + _stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected virtual void GiveData()
    {
        if (_stageData != null)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("NeedLoding");

            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent(out INeedLoding needLoding))
                    needLoding.LodingHandle(_stageData);
            }

            // Change GameState
        }
    }

    protected abstract void NotifyHandleChild();
}
