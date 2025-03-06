using AH.SaveSystem;
using UnityEngine;

public class NPC : InteractionObject
{
    [SerializeField] private NPCSO _npcSO;

    private GameObject _visual;
    private StageSaveDataSO _lastStageDataSO;
    private Collider _col;

    [HideInInspector] public int startIndex;
    [HideInInspector] public int endIndex;

    protected override void Awake()
    {
        base.Awake();

        _col = GetComponent<Collider>();
        _visual = transform.Find("Visual").gameObject;
    }

    private void Start()
    {
        startIndex = _npcSO.startIndex;
        endIndex = _npcSO.endIndex;
        _lastStageDataSO = _npcSO.lastStageDataSO;

        if(!_lastStageDataSO.isClear)
        {
            _col.enabled = false;
            _visual.SetActive(false);
        }
        else
        {
            _col.enabled = true;
            _visual.SetActive(true);
        }
    }
}
