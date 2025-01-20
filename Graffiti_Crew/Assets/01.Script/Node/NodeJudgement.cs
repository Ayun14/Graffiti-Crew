using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    public event Action OnNodeSpawnStart;

    [Header("Spray")]
    [SerializeField] private int _sprayAmount;
    [SerializeField] private int _shakeAmount;

    [Header("Graffiti")]
    [SerializeField] private Sprite _startSprite;

    [Header("Node")]
    [SerializeField] private LayerMask _whatIsNode;
    [SerializeField] private List<NodeDataSO> _nodeDatas;

    public bool isNodeClick => _currentNode != null;

    private NodeSpawner _nodeSpawner;
    private GraffitiRenderer _graffitiRenderer;
    private SprayController _sprayController;

    private Node _currentNode;

    private void Start()
    {
        Init();

        // Test
        NodeSpawnJudgement();
    }

    private void Init()
    {
        _nodeSpawner = GetComponentInChildren<NodeSpawner>();
        _graffitiRenderer = GetComponentInChildren<GraffitiRenderer>();
        _sprayController = GetComponentInChildren<SprayController>();

        // Init
        _nodeSpawner.Init(this, _nodeDatas);
        _graffitiRenderer.Init(this, _startSprite);
        _sprayController.Init(this, _sprayAmount, _shakeAmount);

        _currentNode = null;
    }

    private void Update()
    {
        NodeClickInput();
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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_currentNode != null)
                _currentNode = null;
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

        if (node == _currentNode)
        {
            // Spawn
            NodeSpawnJudgement();

            // Spray
            _sprayController.AddShakeAmount(-_currentNode.GetSprayUseAmount());
            _sprayController.AddSprayAmount(-_currentNode.GetSprayUseAmount());

            _currentNode = null;
        }
    }

    private void NodeSpawnJudgement()
    {
        OnNodeSpawnStart?.Invoke();

        if (_graffitiRenderer != null && _currentNode != null)
            _graffitiRenderer.SetSprite(_currentNode.GetNodeDataSO().graffitiSprite);
    }
}
