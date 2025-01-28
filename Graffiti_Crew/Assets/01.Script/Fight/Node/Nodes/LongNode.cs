using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNode : Node, INodeAction
{
    [SerializeField] private float _cameraDistance;
    [SerializeField] private float _fadeTime;

    private float _sprayUseAmount;

    private LongNodeDataSO _longNodeData;
    private LineRenderer _lineRenderer;
    private LineRenderer _followLineRenderer;
    private SpriteRenderer _startPointRenderer, _endPointRenderer;

    private bool _isFollowingPath = false;
    private int _currentTargetIndex = 0; // ���� ��ǥ�� �ϴ� ����Ʈ�� �ε���
    private List<Vector3> _pathPoints = new List<Vector3>(); // ��� ����Ʈ ����Ʈ

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _startPointRenderer = transform.Find("Start").GetComponent<SpriteRenderer>();
        _endPointRenderer = transform.Find("End").GetComponent<SpriteRenderer>();
        _followLineRenderer = transform.Find("FollowLine").GetComponent<LineRenderer>();
    }

    public override void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(judgement, nodeData);

        _longNodeData = nodeData as LongNodeDataSO;
        _lineRenderer.material = _longNodeData.lineRendererMat;

        _startPointRenderer.sprite = _longNodeData.startNodeSprite;
        _endPointRenderer.sprite = _longNodeData.endNodeSprite;

        _sprayUseAmount = (float)_longNodeData.sprayUseAmount / _longNodeData.points;

        ResetNode();
        SetAlpha(1f);
        ConnectLine();
    }

    #region Connect Line

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
                _pathPoints = StrightLine();
                break;
            case LongNodeType.Curve:
                _pathPoints = CurveLine();
                break;
        }

        StartCoroutine(ConnectLineRoutine(_pathPoints));
    }

    private List<Vector3> StrightLine()
    {
        List<Vector3> strightPoints = new List<Vector3>();

        for (int i = 0; i < _longNodeData.pointList.Count - 1; i++)
        {
            Vector3 start = _longNodeData.pointList[i];
            Vector3 end = _longNodeData.pointList[i + 1];

            for (int j = 0; j < _longNodeData.points; j++)
            {
                float t = (float)j / (_longNodeData.points - 1);
                Vector3 point = Vector3.Lerp(start, end, t);
                strightPoints.Add(point);
            }
        }

        if (strightPoints.Count > 0)
        {
            _startPointRenderer.transform.position = strightPoints[0];
            _endPointRenderer.transform.position = strightPoints[strightPoints.Count - 1];
        }

        return strightPoints;
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

        _startPointRenderer.transform.position = curvePoints[0];
        _endPointRenderer.transform.position = curvePoints[curvePoints.Count - 1];

        return curvePoints;
    }

    private IEnumerator ConnectLineRoutine(List<Vector3> points)
    {
        _lineRenderer.positionCount = points.Count;

        float waitTime = _fadeTime / points.Count;
        for (int i = 0; i < points.Count; ++i)
        {
            // �ڿ������� �̾����� ���̰� �ϱ� ���� ����Ʈ���������� ������ ����Ʈ ��ġ��
            for (int j = i; j < points.Count; ++j)
                _lineRenderer.SetPosition(j, points[i]);
            _endPointRenderer.transform.position = points[i];

            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    public void NodeStartAction()
    {
        if (isClearNode) return;

        _isFollowingPath = true;
        _followLineRenderer.enabled = true;
        _currentTargetIndex = 0;
    }

    #region Clear Check

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isFollowingPath)
            {
                // ��� ���� (�ߵ� ���� ����)
                judgement.NodeFalse(this);
            }

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

            // Move Line
            ConnectFollowLine(_pathPoints.GetRange(0, _currentTargetIndex + 1));

            // ���� ��ǥ ����Ʈ���� �Ÿ� Ȯ��
            if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) < _longNodeData.followThreshold)
            {
                // Combo
                judgement.NodeSuccess(this);

                judgement.AddShakeSliderAmount(-_sprayUseAmount);
                judgement.AddSpraySliderAmount(-_sprayUseAmount);

                // ��� ����Ʈ�� ����ϸ� Ŭ���� ó��
                if (++_currentTargetIndex >= _pathPoints.Count)
                {
                    NodeClear();
                    return;
                }
            }
            else if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) > _longNodeData.failThreshold)
            {
                // ��� ���� (��� ��Ż ����)
                judgement.NodeFalse(this);
                ResetNode();
            }
        }
    }

    private void ResetNode()
    {
        _isFollowingPath = false;
        _followLineRenderer.enabled = false;
        _currentTargetIndex = 0;
    }

    #endregion

    private void ConnectFollowLine(List<Vector3> points)
    {
        _followLineRenderer.positionCount = points.Count;
        _followLineRenderer.SetPositions(points.ToArray());
    }

    public override void NodeClear()
    {
        base.NodeClear();

        if (isClearNode == true) return;
        isClearNode = true;

        _isFollowingPath = false;
        SetAlpha(0f);
    }

    #region Do Fade

    private void SetAlpha(float endValue)
    {
        float startValue = endValue == 1f ? 0f : 1f;

        // ���� �� �ʱ�ȭ
        InitializeAlpha(_startPointRenderer, startValue);
        InitializeAlpha(_endPointRenderer, startValue);
        InitializeAlpha(_lineRenderer.material, startValue);
        InitializeAlpha(_followLineRenderer.material, startValue);

        Sequence fadeSequence = DOTween.Sequence();

        fadeSequence
            // SpriteRenderer
            .Join(_startPointRenderer.DOFade(endValue, _fadeTime))
            .Join(_endPointRenderer.DOFade(endValue, _fadeTime))
            // LineRenderer
            .Join(DOTween.To(() => _lineRenderer.material.color.a,
                x =>
                {
                    Color color = _lineRenderer.material.color;
                    color.a = x;
                    _lineRenderer.material.color = color;
                },
                endValue, _fadeTime))
            .Join(DOTween.To(() => _followLineRenderer.material.color.a,
                x =>
                {
                    Color color = _followLineRenderer.material.color;
                    color.a = x;
                    _followLineRenderer.material.color = color;
                },
                endValue, _fadeTime))
            .OnComplete(() =>
            {
                if (endValue == 0f)
                    pool.Push(this); // Push
            });
    }

    private void InitializeAlpha(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }
    private void InitializeAlpha(Material material, float alpha)
    {
        Color color = material.color;
        color.a = alpha;
        material.color = color;
    }

    #endregion

    public override NodeType GetNodeType()
    {
        return _longNodeData.nodeType;
    }

    public override NodeDataSO GetNodeDataSO()
    {
        return _longNodeData;
    }
}
