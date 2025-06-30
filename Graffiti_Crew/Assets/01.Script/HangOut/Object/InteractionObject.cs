using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AH.SaveSystem;

public class InteractionObject : MonoBehaviour
{
    public BoolSaveDataSO tutorialCheck;
    private bool _checkTutorial = false;

    public PlayerStateEnum stateEnum;
    public Image interactionImg;

    public Vector3 TargetPos => _targetPos;
    private Vector3 _targetPos;

    public Collider Col => _col;
    protected Collider _col;

    protected virtual void Awake()
    {
        _col = GetComponent<Collider>();
        _targetPos = transform.Find("Target").position;
    }

    protected virtual void Start()
    {
        interactionImg.enabled = false;
        if (!tutorialCheck.data && tutorialCheck != null)
            _col.enabled = false;
    }

    protected virtual void Update()
    {
        if (!CheckMousePos())
            interactionImg.enabled = false;

        if (_checkTutorial && tutorialCheck == null) return;

        if (tutorialCheck.data)
        {
            _checkTutorial = true;
            _col.enabled = true;
        }
    }

    private bool CheckMousePos()
    {
        if (_col != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return _col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        }

        return false;
    }
}
