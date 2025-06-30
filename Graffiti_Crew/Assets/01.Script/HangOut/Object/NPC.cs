using AH.SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : InteractionObject
{
    [SerializeField] private NPCSO _npcSO;
    [SerializeField] private GameObject _tutorialPanel;

    private GameObject _visual;
    private StageSaveDataSO _lastStageDataSO;

    [HideInInspector] public int startIndex;
    [HideInInspector] public int endIndex;

    protected override void Awake()
    {
        base.Awake();

        SaveDataEvents.LoadEndEvent += CheckStageData;
        _visual = transform.Find("Visual").gameObject;
    }

    protected override void Start()
    {
        base.Start();

        _col.enabled = true;
    }

    private void OnDisable()
    {
        SaveDataEvents.LoadEndEvent -= CheckStageData;
    }

    private void CheckStageData()
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

        if(tutorialCheck != null && _tutorialPanel != null)
        {
            if (tutorialCheck.data) // Æ©Åä ÈÄ
            {
                CloseTutorialPanel();
                startIndex = 6;
                endIndex = _npcSO.endIndex;
            }
            else // Æ©Åä Àü
            {
                _tutorialPanel.SetActive(true);

                startIndex = _npcSO.startIndex;
                endIndex = 5;
            }
        }    
    }

    public void CloseTutorialPanel()
    {
        _tutorialPanel.SetActive(false);
    }
}
