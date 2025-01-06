using System.Collections.Generic;
using UnityEngine;

public class LongNode : Node
{
    [SerializeField] private LongNodeDataSO _longNodeData;

    private LineRenderer _lineRenderer;
    private Transform _startPoint, _endPoint;

    public override void Init()
    {
        base.Init();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.material = _longNodeData.lineRendererMat;

        // Child
        _startPoint = transform.Find("Start").GetComponent<Transform>();
        _startPoint.GetComponent<SpriteRenderer>().sprite = _longNodeData.nodeSprite;
        _endPoint = transform.Find("End").GetComponent<Transform>();
        _endPoint.GetComponent<SpriteRenderer>().sprite = _longNodeData.nodeSprite;

        ConnectLine();
    }

    private void ConnectLine()
    {
        if (_longNodeData.pointList.Count < 2)
        {
            Debug.LogWarning("Point list의 개수가 2개 이하 임으로 이을 수 없습니다.");
            return;
        }

        switch (_longNodeData.longNodeType)
        {
            case LongNodeType.Stright:
                StrightLine();
                break;
            case LongNodeType.Curve:
                CurveLine();
                break;
        }
    }

    private void StrightLine()
    {
        _lineRenderer.positionCount = _longNodeData.pointList.Count;

        for (int i = 0; i < _longNodeData.points; ++i)
            _lineRenderer.SetPosition(i, _longNodeData.pointList[i]);
    }

    private void CurveLine()
    {
        List<Vector3> curvePoints = new List<Vector3>();
        for (int i = 0; i < _longNodeData.pointList.Count - 1; i++)
        {
            Vector3 start = _longNodeData.pointList[i];
            Vector3 end = _longNodeData.pointList[i + 1];

            // 두 점 사이의 중간 지점 계산
            Vector3 midPoint = (start + end) / 2;

            // 곡선의 진폭 방향으로 offset 추가
            Vector3 direction = (end - start).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
            midPoint += perpendicular * _longNodeData.amplitude;

            // 곡선 생성 Bezier Curve
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
    }

    public override void NodeClear()
    {
        base.NodeClear();

        // 클리어 파티클?
        // 풀매니저에 집어넣기
    }

    public override NodeType GetNodeType()
    {
        return _longNodeData.nodeType;
    }
}
