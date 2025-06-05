using AH.SaveSystem;
using AH.UI.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class DataController : Observer<GameStateController>
{
    [SerializeField] protected LoadStageSO stageSO;
    [HideInInspector] public StageDataSO stageData;

    private int _lodingCnt = 0;
    private List<GameObject> _needLodingObjs;

    private void Awake()
    {
        Attach();
        StageEvent.ClickNectBtnEvent += GoNextStage;
    }

    private void OnDestroy()
    {
        StageEvent.ClickNectBtnEvent -= GoNextStage;
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

    protected void GoNextStage()
    {
        if (stageData.nextChapter != string.Empty && stageData.nextStage != string.Empty)
        {
            stageSO.chapter = stageData.nextChapter;
            stageSO.stage = stageData.nextStage;
            stageSO.SetCurrentStage(stageData.nextChapter + stageData.nextStage, stageData.nextStagetype);
        }

        string nextScene = "ComputerScene";
        switch (stageData.nextStagetype)    
        {
            case StageType.Battle:
                nextScene = "FightScene";
                break;
            case StageType.Activity:
                nextScene = "ActivityScene";
                break;
            case StageType.Story:
                nextScene = "StoryScene";
                break;
        }
        SaveDataEvents.SaveGameEvent?.Invoke(nextScene);

    }
}
