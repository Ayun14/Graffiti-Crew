using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNode : Node, INodeAction
{
    [SerializeField] private float _cameraDistance;

    private LongNodeDataSO _longNodeData;
    private LineRenderer _lineRenderer;
    private LineRenderer _followLineRenderer;
    private SpriteRenderer _startPointRenderer, _endPointRenderer;

    private bool _isFollowingPath = false;
    private int _currentTargetIndex = 0; // 현재 목표로 하는 포인트의 인덱스
    private List<Vector3> _pathPoints = new List<Vector3>(); // 경로 포인트 리스트

    private Sequence _fadeSequence;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _startPointRenderer = transform.Find("Start").GetComponent<SpriteRenderer>();
        _endPointRenderer = transform.Find("End").GetComponent<SpriteRenderer>();
        _followLineRenderer = transform.Find("FollowLine").GetComponent<LineRenderer>();
    }

    public override void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(stageGameRule, judgement, nodeData);

        _longNodeData = nodeData as LongNodeDataSO;
        _lineRenderer.material = _longNodeData.lineRendererMat;

        _startPointRenderer.sprite = _longNodeData.startNodeSprite;
        _endPointRenderer.sprite = _longNodeData.endNodeSprite;
        lastNodePos = _longNodeData.pointList[_longNodeData.pointList.Count - 1];

        NodeReset();
        ConnectLine();
    }

    #region Connect Line

    private void ConnectLine()
    {
        if (_longNodeData.pointList.Count < 2)
        {
            Debug.LogWarning("Point list의 개수가 2개 이하 임으로 이을 수 없습니다.");
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

        if (gameObject.activeInHierarchy)
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
            _startPointRenderer.transform.position = 
                new Vector3(strightPoints[0].x, strightPoints[0].y, _startPointRenderer.transform.position.z);
            
            _endPointRenderer.transform.position = 
                new Vector3(strightPoints[strightPoints.Count - 1].x, 
                strightPoints[strightPoints.Count - 1].y, 
                _endPointRenderer.transform.position.z);
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

        _startPointRenderer.transform.position =
            new Vector3(curvePoints[0].x, curvePoints[0].y, _startPointRenderer.transform.position.z);

        _endPointRenderer.transform.position =
            new Vector3(curvePoints[curvePoints.Count - 1].x,
            curvePoints[curvePoints.Count - 1].y,
            _endPointRenderer.transform.position.z);

        return curvePoints;
    }

    private IEnumerator ConnectLineRoutine(List<Vector3> points)
    {
        _lineRenderer.positionCount = points.Count;

        float waitTime = fadeTime / points.Count;
        for (int i = 0; i < points.Count; ++i)
        {
            // 자연스럽게 이어지게 보이게 하기 위해 포인트들을현재의 마지막 포인트 위치로
            for (int j = i; j < points.Count; ++j)
                _lineRenderer.SetPosition(j, points[i]);

            _endPointRenderer.transform.position = new Vector3(points[i].x, points[i].y, 
                _endPointRenderer.transform.position.z);

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

        // Sound
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Spray_Long);
    }

    #region Clear Check

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isFollowingPath)
            {
                // 노드 실패 (중도 포기 실패)
                NodeFalse();

                // Sound
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Spray_Long);
            }

            NodeReset();
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

            // 현재 목표 포인트와의 거리 확인
            if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) < _longNodeData.followThreshold)
            {
                // Particle
                PopGraffitiParticle(mouseWorldPosition + new Vector3(0f, 0f, -0.1f));

                // Spray
                _stageGameRule.AddShakeSliderAmount(-_longNodeData.sprayUseAmount / _pathPoints.Count);
                _stageGameRule.AddSpraySliderAmount(-_longNodeData.sprayUseAmount / _pathPoints.Count);

                // 모든 포인트를 통과하면 클리어 처리
                if (++_currentTargetIndex >= _pathPoints.Count)
                {
                    NodeClear();
                    return;
                }
            }
            else if (Vector3.Distance(mouseWorldPosition, _pathPoints[_currentTargetIndex]) > _longNodeData.failThreshold)
            {
                // 노드 실패 (경로 이탈 실패)
                NodeFalse();
            }
        }
    }

    public override void NodeFalse()
    {
        _judgement.NodeFalse();
        NodeReset();
    }

    public override void NodeReset()
    {
        _isFollowingPath = false;
        _followLineRenderer.enabled = false;
        _currentTargetIndex = 0;

        // Sound
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Spray_Long);
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

        // Sound
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Spray_Long);

        SetAlpha(0f, fadeTime, () => pool.Push(this));
    }

    #region Do Fade

    public override void SetAlpha(float endValue, float time = 0, Action callback = null)
    {
        float startValue = endValue == 1f ? 0f : 1f;

        // 알파 값 초기화
        InitializeAlpha(_startPointRenderer, startValue);
        InitializeAlpha(_endPointRenderer, startValue);
        InitializeAlpha(_lineRenderer.material, startValue);
        InitializeAlpha(_followLineRenderer.material, startValue);

        if (_fadeSequence.IsActive()) _fadeSequence.Complete();
        _fadeSequence = DOTween.Sequence();

        _fadeSequence
            // SpriteRenderer
            .Join(_startPointRenderer.DOFade(endValue, fadeTime))
            .Join(_endPointRenderer.DOFade(endValue, fadeTime))
            // LineRenderer
            .Join(DOTween.To(() => _lineRenderer.material.color.a,
                x =>
                {
                    Color color = _lineRenderer.material.color;
                    color.a = x;
                    _lineRenderer.material.color = color;
                },
                endValue, fadeTime))
            .Join(DOTween.To(() => _followLineRenderer.material.color.a,
                x =>
                {
                    Color color = _followLineRenderer.material.color;
                    color.a = x;
                    _followLineRenderer.material.color = color;
                },
                endValue, fadeTime))
            .OnComplete(() =>
            {
                callback?.Invoke();
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
