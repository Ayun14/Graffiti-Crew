using UnityEngine;

public class NPC : InteractionObject
{
    [SerializeField] private StageSaveDataSO _lastStageDataSO;
    private GameObject _visual;

    public int startIndex;
    public int endIndex;

    protected override void Awake()
    {
        base.Awake();

        _visual = transform.Find("Visual").gameObject;
    }

    private void Start()
    {
        if(!_lastStageDataSO.isClear)
            _visual.SetActive(false);
        else
            _visual.SetActive(true);
    }
}
