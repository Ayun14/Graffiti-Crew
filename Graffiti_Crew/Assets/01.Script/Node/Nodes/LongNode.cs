using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LongNode : Node
{
    [SerializeField] private LongNodeDataSO _longNodeData;
    [SerializeField] private float _cameraDistance;

    private LineRenderer _lineRenderer;
    private Transform _startPoint, _endPoint;

    private bool _isFollowingPath = false;
    private int _currentTargetIndex = 0; // ���� ��ǥ�� �ϴ� ����Ʈ�� �ε���
    private List<Vector3> _pathPoints = new List<Vector3>(); // ��� ����Ʈ ����Ʈ

    public override void Init()
    {
        base.Init();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.material = _longNodeData.lineRendererMat;

        // Child
        _startPoint = transform.Find("Start").GetComponent<Transform>();
        _startPoint.GetComponent<SpriteRenderer>().sprite = _longNodeData.startNodeSprite;
        _endPoint = transform.Find("End").GetComponent<Transform>();
        _endPoint.GetComponent<SpriteRenderer>().sprite = _longNodeData.endNodeSprite;

        ConnectLine();
    }

    #region ConnectLine

    private void ConnectLine()
    {
        if (_longNodeData.pointList.Count < 2)
        {
            Debug.LogWarning("Point list�� ������ 2�� ���� ������ ���� �� �����ϴ�.");
            return;
        }

        _pathPoints.Clear();
        switch (_longNodeData.longNodeType)
        {
            case LongNodeType.Stright:
                _pathPoints.AddRange(_longNodeData.pointList);
                StrightLine();
                break;
            case LongNodeType.Curve:
                _pathPoints = CurveLine();    
                break;
        }
    }

    private void StrightLine()
    {
        _lineRenderer.positionCount = _longNodeData.pointList.Count;

        for (int i = 0; i < _longNodeData.points; ++i)
            _lineRenderer.SetPosition(i, _longNodeData.pointList[i]);
    }

    private List<Vector3> CurveLine()
    {
        List<Vector3> curvePoints = new List<Vector3>();
        for (int i = 0; i < _longNodeData.pointList.Count - 1; i++)
        {
            Vector3 start = _longNodeData.pointList[i];
            Vector3 end = _longNodeData.pointList[i + 1];

            // �� �� ������ �߰� ���� ���
            Vector3 midPoint = (start + end) / 2;

            // ��� ���� �������� offset �߰�
            Vector3 direction = (end - start).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
            midPoint += perpendicular * _longNodeData.amplitude;

            // � ���� Bezier Curve
            for (int j = 0; j <= _longNodeData.points; j++)
            {
                float t = j / (float)_longNodeData.points;

                Vector3 curvePoint = Mathf.Pow(1 - t, 2) * start +
                                     2 * (1 - t) * t * midPoint +
                                     Mathf.Pow(t, 2) * end;

                curvePoints.Add(curvePoint);
            }
        }

        _startPoint.position = curvePoints[0];
        _endPoint.position = curvePoints[curvePoints.Count - 1];

        _lineRenderer.positionCount = curvePoints.Count;
        for (int i = 0; i < curvePoints.Count; i++)
            _lineRenderer.SetPosition(i, curvePoints[i]);

        return curvePoints;
    }

    #endregion

    #region Clear Check

    public void LongNodeStart()
    {
        _isFollowingPath = true;
        _currentTargetIndex = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("�� Ŭ������ �ʾ� ����");
            ResetNode();
        }

        CheckFollowingPath();
    }

    private void CheckFollowingPath()
    {
        if (_isFollowingPath)
        {
            Vector3 mouosePos = Input.mousePosition;
            mouosePos.z = _cameraDistance;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouosePos);
            mouseWorldPosition.z = transform.position.z;

            // ���� ��ǥ ����Ʈ���� �Ÿ� Ȯ��
            if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) < _longNodeData.followThreshold)
            {
                Mathf.Clamp(0, _pathPoints.Count - 1, ++_currentTargetIndex);

                // ��� ����Ʈ�� ����ϸ� Ŭ���� ó��
                if (_currentTargetIndex >= _pathPoints.Count)
                {
                    NodeClear();
                    return;
                }
            }
            else if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) > _longNodeData.failThreshold)
            {
                Debug.Log("��� ��Ż ����");
                ResetNode();
            }
        }
    }

    private void ResetNode()
    {
        _isFollowingPath = false;
        _currentTargetIndex = 0;
    }

    #endregion

    public override void NodeClear()
    {
        base.NodeClear();

        Debug.Log("Long Node Clear");
        // Ŭ���� ��ƼŬ?
        // Ǯ�Ŵ����� ����ֱ�
    }

    public override NodeType GetNodeType()
    {
        return _longNodeData.nodeType;
    }


    private void OnDrawGizmos()
    {
        if (_pathPoints.Count > 0 && _currentTargetIndex < _pathPoints.Count)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_pathPoints[_currentTargetIndex], 0.1f); // ���� ��ǥ ����Ʈ ǥ��
        }

        Vector3 mouosePos = Input.mousePosition;
        mouosePos.z = _cameraDistance;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouosePos);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mouseWorldPosition, 0.1f); // ���콺 ��ġ ǥ��
    }
}
