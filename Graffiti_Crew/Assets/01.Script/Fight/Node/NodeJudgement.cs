using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private NodeSpawner _nodeSpawner;
    private GraffitiRenderer _graffitiRenderer;
    private SprayController _sprayController;
    private ComboController _comboController;

    [HideInInspector] public StageResultSO stageResult;

    private Node _currentNode;

    public void LodingHandle(StageDataSO stageData)
    {
        stageResult = stageData.stageResult;
        _startSprite = stageData.startGraffiti;
        _nodeDatas = stageData.nodeDatas;
    }

    private void Awake()
    {
        Attach();

        mySubject.OnSprayChangeEvent += SprayChangeEventHandle;

        Init();
    }

    private void Start()
    {
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
        if (mySubject.GameState == GameState.Fight || mySubject.GameState == GameState.Graffiti)
        {
            // Init
            _nodeSpawner.Init(this, _nodeDatas);
            _graffitiRenderer.Init(this, _startSprite);
            _sprayController.Init(this);
            _comboController.Init(this);

            NodeSpawnJudgement();
        }
    }

    #region Input

    private void NodeClickInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (mySubject.IsSprayEmpty || _sprayController.isMustShakeSpray) return;

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
            else // HitNode Combo ½ÇÆÐ
            {
                if (_currentNode != null && _currentNode.GetNodeType() == NodeType.HitNode)
                    NodeFalse(_currentNode);
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

    public void AllNodeClear()
    {
        mySubject.SetWhoIsWin(true);
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
                mySubject?.InvokeBlindEvent();
        }

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
