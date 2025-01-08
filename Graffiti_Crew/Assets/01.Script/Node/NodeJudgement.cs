using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    public event Action OnNodeSpawnStart;

    [SerializeField] private LayerMask _whatIsNode;
    [SerializeField] private List<GameObject> _nodes;

    private NodeSpawner _spawner;
    private Node _currentNode;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _spawner = GetComponentInChildren<NodeSpawner>();

        _spawner.SetSpawnNode(_nodes);
        _currentNode = null;

    }

    private void Start()
    {
        // Test
        OnNodeSpawnStart?.Invoke();
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
            _currentNode = null;
            OnNodeSpawnStart?.Invoke();
        }
    }
}
