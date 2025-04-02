using AH.UI.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NodeJudgement : Observer<GameStateController>, INeedLoding
{
    public event Action OnNodeSpawnStart;

    [Header("Player")]
    [SerializeField] private SliderValueSO _playerSliderValueSO;
    private int _nodeCnt => _nodeDatas.Count;
    private int _clearNodeCnt = 0;

    // Graffiti
    private Sprite _startSprite;

    [Header("Node")]
    [HideInInspector] public bool isNodeClick;
    [SerializeField] private LayerMask _whatIsNode;
    private List<NodeDataSO> _nodeDatas;

    public int ClearNodeCnt => _clearNodeCnt;

    private NodeSpawner _nodeSpawner;
    private GraffitiRenderer _graffitiRenderer;
    private SprayController _sprayController;
    private ComboController _comboController;

    [HideInInspector] public StageResultSO stageResult;

    private Node _currentNode;

    public void LodingHandle(DataController dataController)
    {
        stageResult = dataController.stageData.stageResult;
        _startSprite = dataController.stageData.startGraffiti;
        _nodeDatas = dataController.stageData.nodeDatas;

        dataController.SuccessGiveData();
    }

    private void Awake()
    {
        Attach();

        mySubject.OnSprayChangeEvent += SprayChangeEventHandle;

        Init();
    }

    private void Update()
    {
        NodeClickInput();
    }

    private void OnDestroy()
    {
        mySubject.OnSprayChangeEvent -= SprayChangeEventHandle;

        Detach();
    }

    private void Init()
    {
        _nodeSpawner = GetComponentInChildren<NodeSpawner>();
        _graffitiRenderer = GetComponentInChildren<GraffitiRenderer>();
        _sprayController = GetComponentInChildren<SprayController>();
        _comboController = GetComponentInChildren<ComboController>();

        _currentNode = null;
        _clearNodeCnt = 0;

        _playerSliderValueSO.Value = _playerSliderValueSO.min;
    }

    public override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Timeline || mySubject.GameState == GameState.Talk)
            _graffitiRenderer.Init(this, _startSprite);

        if (mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Graffiti
            || mySubject.GameState == GameState.Tutorial)
        {
            // Init
            _graffitiRenderer.Init(this, _startSprite); // 나중에 지우기
            _nodeSpawner.Init(this, _nodeDatas);
            _sprayController.Init(this);
            _comboController.Init(this);

            NodeSpawnJudgement();
        }

        if (mySubject.GameState == GameState.Finish) _nodeSpawner.StopSpawn();
    }

    #region Input

    private void NodeClickInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (mySubject.IsSprayEmpty || _sprayController.isMustShakeSpray) return;
        if (mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Graffiti
            || mySubject.GameState == GameState.Tutorial)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
                {
                    if (hit.transform.parent.TryGetComponent(out Node node))
                    {
                        isNodeClick = true;

                        _currentNode = node;
                        NodeClick(_currentNode);
                    }
                }
                else // HitNode Combo 실패
                {
                    if (_currentNode != null && _currentNode.GetNodeType() == NodeType.HitNode)
                        NodeFalse(_currentNode);
                }
            }
        }
    }

    private void NodeClick(Node node)
    {
        if (node is INodeAction actionNode)
        {
            actionNode.NodeStartAction();
        }
    }

    #endregion

    #region Node Clear

    public void CheckNodeClear(Node node)
    {
        if (node == null || _currentNode == null) return;

        // NodeClear
        if (node == _currentNode)
        {
            // Player Slider Update
            float percent = ++_clearNodeCnt / (float)_nodeCnt;
            _playerSliderValueSO.Value = _playerSliderValueSO.max * percent;

            // Spawn
            NodeSpawnJudgement();

            _currentNode = null;
        }
    }

    private void NodeSpawnJudgement()
    {
        OnNodeSpawnStart?.Invoke();

        if (_graffitiRenderer != null && _currentNode != null)
            _graffitiRenderer.SetSprite(_currentNode.GetNodeDataSO().graffitiSprite);
    }

    public async void AllNodeClear()
    {
        mySubject.SetWhoIsWin(true);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("TutorialScene"))
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Dialogue);
        }
        else
            mySubject.ChangeGameState(GameState.Finish);

    }

    #endregion

    #region Combo

    public void NodeSuccess(Node node)
    {
        if (node == null) return;

        // Combo
        _comboController.SuccessCombo();
    }

    public void NodeFalse(Node node)
    {
        if (node == null) return;

        if (stageResult != null)
            stageResult.nodeFalseCnt++;

        if (node.GetNodeType() == NodeType.LongNode)
        {
            if (!mySubject.IsBlind && Random.Range(0, 100f) < 30)
            {
                mySubject?.InvokeBlindEvent();

                // Sound
                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Throw_Egg);
            }
        }

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Spray_Miss, false, Random.Range(0.8f, 1.2f));

        mySubject.InvokeNodeFailEvent();

        _comboController.FailCombo();
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

    public void SprayEmptyEvent()
    {
        if (mySubject.IsSprayEmpty) return;

        mySubject.SetIsSprayEmpty(true);
        mySubject?.InvokeSprayEmptyEvent();
    }

    private void SprayChangeEventHandle()
    {
        _sprayController.SprayChange();
    }

    #endregion
}
