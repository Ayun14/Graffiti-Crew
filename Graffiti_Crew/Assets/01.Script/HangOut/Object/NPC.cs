using AH.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : InteractionObject
{
    [SerializeField] protected NPCSO _npcSO;

    private GameObject _visual;
    private StageSaveDataSO _lastStageDataSO;

    public int StartIndex => _startIndex;
    public int EndIndex => _endIndex;

    protected int _startIndex;
    protected int _endIndex;

    protected override void Awake()
    {
        base.Awake();

        SaveDataEvents.LoadEndEvent += CheckStageData;
        _visual = transform.Find("Visual").gameObject;
    }

    protected override void Start()
    {
        interactionImg.enabled = false;

        _startIndex = _npcSO.startIndex;
        _endIndex = _npcSO.endIndex;
    }

    private void OnDisable()
    {
        SaveDataEvents.LoadEndEvent -= CheckStageData;
    }

    protected virtual void CheckStageData()
    {
        if (_npcSO.lastStageDataSO != null)
        {
            _lastStageDataSO = _npcSO.lastStageDataSO;
            if (_lastStageDataSO.stageState != StageState.Clear)
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
}
