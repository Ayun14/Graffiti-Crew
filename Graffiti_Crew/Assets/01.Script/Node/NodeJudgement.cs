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
                LongNode longNode = node as LongNode;
                longNode.LongNodeStart();
                //node.NodeClear();
                break;
        }
    }
}
