using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeJudgement : Observer<GameStateController>, INeedLoding
{
    public event Action OnNodeSpawnStart;

    [Header("Spray")]
    [SerializeField] private int _sprayAmount;
    [SerializeField] private int _shakeAmount;

    // Graffiti
    private Sprite _startSprite;

    [Header("Node")]
    [SerializeField] private LayerMask _whatIsNode;
    private List<NodeDataSO> _nodeDatas;

    [HideInInspector] public bool isNodeClick => _currentNode != null;

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

    private void Start()
    {
        Attach();

        Init();
    }

    private void Init()
    {
        _nodeSpawner = GetComponentInChildren<NodeSpawner>();
        _graffitiRenderer = GetComponentInChildren<GraffitiRenderer>();
        _sprayController = GetComponentInChildren<SprayController>();
        _comboController = GetComponentInChildren<ComboController>();

        _currentNode = null;
    }

    private void Update()
    {
        NodeClickInput();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject.GameState == GameState.Fight)
        {
            // Init
            _nodeSpawner.Init(this, _nodeDatas);
            _graffitiRenderer.Init(this, _startSprite);
            _sprayController.Init(this, _sprayAmount, _shakeAmount);
            _comboController.Init(this);

            NodeSpawnJudgement();
        }
    }

    private void NodeClickInput()
    {
        if (_sprayController.isSprayNone || _sprayController.isMustShakeSpray) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
            {
                if (hit.transform.parent.TryGetComponent(out Node node))
                {
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

    public void CheckNodeClear(Node node)
    {
        if (node == null || _currentNode == null) return;

        // NodeClear
        if (node == _currentNode)
        {
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
        mySubject.ChangeGameState(GameState.Finish);
    }

    public void AddShakeSliderAmount(float value)
    {
        _sprayController.AddShakeAmount(value);
    }

    public void AddSpraySliderAmount(float value)
    {
        _sprayController.AddSprayAmount(value);
    }

    public void NodeSuccess(Node node)
    {
        if (node == null) return;

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
}
