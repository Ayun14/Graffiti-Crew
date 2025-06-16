using AH.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if(CheckHangOutScene())
        {
            _col = GetComponent<Collider>();
            _visual = transform.Find("Visual").gameObject;
        }
    }

    protected override void Start()
    {
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

            if (tutorialCheck.data) // Æ©Åä ÈÄ
            {
                startIndex = _npcSO.startIndex;
                endIndex = 5;
            }
            else // Æ©Åä Àü
            {
                startIndex = 6;
                endIndex = _npcSO.endIndex;
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
