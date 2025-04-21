using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StageGameRule : Observer<GameStateController>
{
    public StageRuleType stageRule;

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
        if (mySubject.IsSprayEmpty || _sprayController.isMustShakeSpray) return false;

        return mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Graffiti
            || mySubject.GameState == GameState.Tutorial;
    }

    public void NodeSpawn() => _nodeSpawner.NodeSpawn();
    public NodeDataSO GetCurrentNode() => _nodeSpawner.PeekNode();

    #region Loding And Notify

    public void Loding(DataController dataController)
    {
        stageResult = dataController.stageData.stageResult;
        _startSprite = dataController.stageData.startGraffiti;
        _nodeDatas = dataController.stageData.nodeDatas;

        // Judgement And Spawner
        _nodeSpawner = Instantiate(dataController.stageData.spawnerPrefab, transform).GetComponentInChildren<NodeSpawner>();
        _nodeJudgement = Instantiate(dataController.stageData.judgementPrefab, transform).GetComponentInChildren<NodeJudgement>();
        _nodeJudgement.Init(this);
    }

    public override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Timeline || mySubject.GameState == GameState.Talk)
            _graffitiRenderer.Init(this, _startSprite);

        if (mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Graffiti
            || mySubject.GameState == GameState.Tutorial)
        {
            // Init
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
        _comboController.SuccessCombo();
    }

    public virtual void NodeFalse()
    {
        if (stageResult != null && stageRule == StageRuleType.OneTouchRule)
            stageResult.value++;

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Miss, Random.Range(0.8f, 1.2f));
        mySubject.InvokeNodeFailEvent();
        _comboController.FailCombo();
    }

    public async void AllNodeClear()
    {
        if (mySubject.GameState == GameState.Fight)
        {
            mySubject.SetWhoIsWin(true);
            mySubject.ChangeGameState(GameState.Finish);
        }
        else if (mySubject.GameState == GameState.Graffiti)
            mySubject.ChangeGameState(GameState.Talk);
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

        mySubject.SetIsSprayEmpty(isEmpty);
    }

    #endregion
}