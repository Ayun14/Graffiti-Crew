using TMPro;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    private int _currentCombo = 0;
    private int _allCombo = 0;

    private TextMeshProUGUI _comboText;
    private NodeJudgement _judgement;

    private void Awake()
    {
        Transform parent = transform.Find("Panel_Combo");
        _comboText = parent.Find("Text_Combo").GetComponent<TextMeshProUGUI>();
    }

    public void Init(NodeJudgement judgement)
    {
        _judgement = judgement;
        _currentCombo = 0;

        if (_comboText != null)
        {
            _comboText.text = string.Empty;
        }
    }

    public void SuccessCombo()
    {
        ComboTextUpdate(++_currentCombo);
    }

    public void FailCombo()
    {
        Debug.Log("Fail");
        _allCombo += _currentCombo;
        _currentCombo = 0;

        ComboTextUpdate(_currentCombo);
    }

    private void ComboTextUpdate(int combo)
    {
        if (_comboText == null) return;

        _comboText.text = "Combo! " + combo.ToString();
    }
}
