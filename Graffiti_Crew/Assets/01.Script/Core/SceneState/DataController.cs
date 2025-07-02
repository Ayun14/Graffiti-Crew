using AH.SaveSystem;
using AH.UI.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DataController : Observer<GameStateController>
{
    [SerializeField] protected LoadStageSO stageSO;
    [HideInInspector] public StageDataSO stageData;

    [Header("Loding UI")]
    [SerializeField] private TMIDataReader _tmiDataReader;
    [SerializeField] private GameObject _lodingPanel;
    private TextMeshProUGUI _tmiText;
    private Slider _lodingSlider;

    private int _lodingCnt = 0;
    private List<GameObject> _needLodingObjs;

    protected virtual void Awake()
    {
        Attach();
        StageEvent.ClickNectBtnEvent += GoNextStage;

        _tmiText = _lodingPanel.transform.Find("TMI").transform.Find("Text").GetComponent<TextMeshProUGUI>();
        _lodingSlider = _lodingPanel.GetComponentInChildren<Slider>();
        _lodingSlider.value = 0;
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
            StartCoroutine(LodingRoutine(1f, Random.Range(1f, 1.2f)));
    }

    private IEnumerator LodingRoutine(float value, float time)
    {
        // TODO: Set TMI
        int randIndex = Random.Range(0, _tmiDataReader.SprayList.Count);
        if (_tmiText != null)
            _tmiText.text = _tmiDataReader.SprayList[randIndex].tmi;

        if (_lodingSlider != null)
        {
            // Slider Update
            float currentTime = 0;
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                float t = currentTime / time;
                _lodingSlider.value = Mathf.Lerp(0, value, t);
                yield return null;
            }
            _lodingSlider.value = value;
        }
        FinishGiveData();
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
        if (stageData.stageType == StageType.Story || stageData.isPlayerWin)
        {
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
        }
        SaveDataEvents.SaveGameEvent?.Invoke(nextScene);
    }
}
