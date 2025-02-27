using AH.Map;
using UnityEngine;

public abstract class DataController : Observer<GameStateController>
{
    [SerializeField] protected LoadStageSO stageSO;

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

    protected abstract void FindDatas();

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
