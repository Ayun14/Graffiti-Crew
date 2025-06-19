using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StageGameRule : Observer<GameStateController>
{
    // Tutorial
    public event Action OnNodeClear;
    [HideInInspector] public bool isTurotial = false; // Input

    public StageRuleType stageRule;
    protected StageType _stageType;

    // Children
    protected NodeJudgement _nodeJudgement;
    protected NodeSpawner _nodeSpawner;
    protected GraffitiRenderer _graffitiRenderer;
    protected SprayController _sprayController;
    protected ComboController _comboController;

    // Children Data
    [HideInInspector] public StageResultSO stageResult;
    protected Sprite _startSprite;
    protected List<NodeDataSO> _nodeDatas;
    public int NodeCnt => _nodeDatas.Count;

    protected virtual void Awake()
    {
        Attach();

        FindChildren();
    }

    protected virtual void FindChildren()
    {
        _graffitiRenderer = GetComponentInChildren<GraffitiRenderer>();
        _sprayController = GetComponentInChildren<SprayController>();
        _comboController = GetComponentInChildren<ComboController>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public bool IsCanInput()
    {
        if (isTurotial) return false;
        if (mySubject.IsSprayEmpty || _sprayController.isMustShakeSpray) return false;

        return mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Tutorial;
    }

    public void NodeSpawn() => _nodeSpawner.NodeSpawn();
    public NodeDataSO GetCurrentNode() => _nodeSpawner.PeekNode();

    #region Loding And Notify

    public void Loding(DataController dataController)
    {
        stageResult = dataController.stageData.stageResult;
        _startSprite = dataController.stageData.memberGraffiti;
        _nodeDatas = dataController.stageData.nodeDatas;
        _stageType = dataController.stageData.stagetype;

        // Judgement And Spawner
        _nodeSpawner = Instantiate(dataController.stageData.spawnerPrefab, transform).GetComponentInChildren<NodeSpawner>();
        _nodeJudgement = Instantiate(dataController.stageData.judgementPrefab, transform).GetComponentInChildren<NodeJudgement>();
        _nodeJudgement.Init(this);
    }

    public override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Tutorial)
        {
            // Init
            _graffitiRenderer.Init(this, _startSprite);
            _nodeSpawner.Init(this, _nodeJudgement, _nodeDatas);
            _sprayController.Init(this);
            _comboController.Init(this);

            _nodeSpawner.NodeSpawn();
        }

        if (mySubject.GameState == GameState.Finish) _nodeSpawner.StopSpawn();
    }

    #endregion

    #region Node Clear Check

    public virtual void NodeClear()
    {
        // Combo
        //_comboController.SuccessCombo();

        OnNodeClear?.Invoke();
    }

    public virtual void NodeFalse()
    {
        if (stageResult != null && stageRule == StageRuleType.OneTouchRule)
            stageResult.value++;

        // Spray
        if (_stageType == StageType.Activity)
            _sprayController.AddSprayAmount(-30f);

        // Combo
        //_comboController.FailCombo();

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Miss, UnityEngine.Random.Range(0.8f, 1.2f));

        mySubject.InvokeNodeFailEvent();
    }

    public void PlayerLoseCheck()
    {
        if (_stageType == StageType.Activity)
        {
            mySubject.SetWhoIsWin(false);
            mySubject.ChangeGameState(GameState.Finish);
        }
    }

    public async void AllNodeClear()
    {
        _nodeSpawner.StopSpawn();

        if (mySubject.GameState == GameState.Fight)
        {
            mySubject.SetWhoIsWin(true);
            mySubject.ChangeGameState(GameState.Finish);
        }
        else if (mySubject.GameState == GameState.Tutorial)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Dialogue);
        }
    }

    #endregion

    #region Spray Event

    public void AddShakeSliderAmount(float value)
    {
        _sprayController.AddShakeAmount(value);
    }

    public void AddSpraySliderAmount(float value)
    {
        _sprayController.AddSprayAmount(value);
    }

    public void SetSprayEmpty(bool isEmpty)
    {
        if (mySubject.IsSprayEmpty == isEmpty) return;

        _nodeJudgement.CurrentNodeReset();
        mySubject.SetIsSprayEmpty(isEmpty);
    }

    public bool GetSprayEmpty() => mySubject.IsSprayEmpty;

    #endregion
}