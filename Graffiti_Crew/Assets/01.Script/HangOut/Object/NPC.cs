using UnityEngine;

public class NPC : InteractionObject
{
    [SerializeField] private NPCSO _npcSO;

    private GameObject _visual;
    private StageSaveDataSO _lastStageDataSO;
    [HideInInspector] public int startIndex;
    [HideInInspector] public int endIndex;

    protected override void Awake()
    {
        base.Awake();

        _visual = transform.Find("Visual").gameObject;
    }

    private void Start()
    {
        startIndex = _npcSO.startIndex;
        endIndex = _npcSO.endIndex;
        _lastStageDataSO = _npcSO.lastStageDataSO;

        if(!_lastStageDataSO.isClear)
            _visual.SetActive(false);
        else
            _visual.SetActive(true);
    }
}
