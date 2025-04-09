using UnityEngine;

public class MemoryJudgement : NodeJudgement
{
    protected override void NodeInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (_stageGameRule.IsCanInput() && Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
            {
                if (hit.transform.parent.TryGetComponent(out Node node))
                {
                    // node �� ���� ���� �տ� �ִ� ��� ���?
                    if (_stageGameRule.GetCurrentNode() == node.GetNodeDataSO())
                    {
                        isNodeClick = true;
                        currentNode = node;
                        NodeClick(currentNode);
                    }
                    else
                    {
                        // Ŭ�� ���� ��� ���� ��������
                        _stageGameRule.NodeSpawn();
                    }
                }
            }
            else
            {
                // HitNode Combo ����
                if (currentNode != null && currentNode.GetNodeType() == NodeType.HitNode)
                    _stageGameRule.NodeFalse(currentNode);

                // Ŭ�� ���� ��� ���� ��������
                _stageGameRule.NodeSpawn();
            }
        }
    }
}
