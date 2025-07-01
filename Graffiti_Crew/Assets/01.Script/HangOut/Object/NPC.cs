using AH.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : InteractionObject
{
    [SerializeField] protected NPCSO _npcSO;

    private GameObject _visual;

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
            Debug.Log(_npcSO.lastStageDataSO);
            Debug.Log(_npcSO.lastStageDataSO.stageState);
            if (_npcSO.lastStageDataSO.stageState != StageState.Clear)
            {
                Debug.Log("No");
                _col.enabled = false;
                _visual.SetActive(false);
            }
            else
            {
                Debug.Log("Clear");
                _col.enabled = true;
                _visual.SetActive(true);
            }
        }
    }
}
