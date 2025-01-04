using UnityEngine;

public class NodeJudgement : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsNode;

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
                Debug.Log("Hit Object: " + hit.collider.name);

                if (hit.transform.parent.TryGetComponent(out Node node))
                {
                    NodeClick(node);
                }
            }
            else Debug.Log("No object hit.");
        }
    }

    private void NodeClick(Node node)
    {
        switch (node.GetNodeType())
        {
            case NodeType.SingleNode:
                node.NodeClear();
                break;
            case NodeType.HitNode:
                HitNode hitNode = node as HitNode;
                hitNode.SetHitCount();
                break;
            case NodeType.LongNode:
                // 길게 눌렀을 때 어떻게 할 것 인지
                node.NodeClear();
                break;
        }
    }
}
