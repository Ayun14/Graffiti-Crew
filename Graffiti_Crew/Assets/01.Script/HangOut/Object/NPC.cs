using AH.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if(CheckHangOutScene())
        {
            _col = GetComponent<Collider>();
            _visual = transform.Find("Visual").gameObject;
        }
    }

    private void Start()
    {
        startIndex = _npcSO.startIndex;
        endIndex = _npcSO.endIndex;

        SaveDataEvents.LoadEndEvent += CheckStageData;
    }

    private void OnDisable()
    {
        SaveDataEvents.LoadEndEvent -= CheckStageData;
    }

    private void CheckStageData()
    {
        if (_npcSO.lastStageDataSO != null && CheckHangOutScene())
        {
            _lastStageDataSO = _npcSO.lastStageDataSO;
            if (!_lastStageDataSO.isClear)
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

    private bool CheckHangOutScene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("HangOutScene"))
            return true;
        else
            return false;
    }
}
