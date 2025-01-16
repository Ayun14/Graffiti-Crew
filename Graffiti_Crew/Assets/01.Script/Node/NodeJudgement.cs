using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    public event Action OnNodeSpawnStart;

    [SerializeField] private LayerMask _whatIsNode;
    [SerializeField] private List<NodeDataSO> _nodeDatas;

    private NodeSpawner _nodeSpawner;
    private GraffitiRenderer _graffitiRenderer;
    private Node _currentNode;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _nodeSpawner = GetComponentInChildren<NodeSpawner>();
        _graffitiRenderer = GetComponentInChildren<GraffitiRenderer>();

        _nodeSpawner.SetSpawnNode(_nodeDatas);
        _currentNode = null;
    }

    private void Start()
    {
        // Test
        NodeSpawnJudgement();
    }

    private void Update()
    {
        NodeClickInput();
    }

    private void NodeClickInput()
    {
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
}
