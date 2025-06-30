using AH.SaveSystem;
using UnityEngine;

public class Ella : NPC
{
    [SerializeField] private GameObject _tutorialPanel;

    protected override void Start()
    {
        interactionImg.enabled = false;
    }

    protected override void CheckStageData()
    {
        if (tutorialCheck != null && _tutorialPanel != null)
        {
            _col.enabled = true;

            if (tutorialCheck.data) // Æ©Åä ÈÄ
            {
                CloseTutorialPanel();

                _startIndex = 6;
                _endIndex = _npcSO.endIndex;
            }
            else // Æ©Åä Àü
            {
                HangOutEvent.SetPlayerMovementEvent?.Invoke(false);
                _tutorialPanel.SetActive(true);

                _startIndex = _npcSO.startIndex;
                _endIndex = 5;
            }
        }
    }

    public void CloseTutorialPanel()
    {
        if (_tutorialPanel == null) return;

        _tutorialPanel.SetActive(false);
        HangOutEvent.SetPlayerMovementEvent?.Invoke(true);
    }
}
