using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum BGType
{
    None,
    FadeIn,
    FadeOut,
    FlashIn,
    FlashOut,
    ShowCutScene,
    HideCutScene,
    ShakeCam,
    Animation
}

public enum ActionType
{
    None,
    JiaMove,
    JiaStop,
}

[Serializable]
public struct DialogueData
{
    public int id;
    public string characterName;
    [TextArea] public string context;
    public string spriteName;
    public string soundName;
    public string animName;
    public BGType bgType;
    public ActionType actionType;

    public DialogueData(int id, string characterName, string context, string spriteName, string soundName, string animName, BGType bGType, ActionType actionType)
    {
        this.id = id;
        this.characterName = characterName;
        this.context = context;
        this.spriteName = spriteName;
        this.soundName = soundName;
        this.animName = animName;
        this.bgType = bGType;
        this.actionType = actionType;
    }
}

[CreateAssetMenu(fileName = "DialogueReader", menuName = "SO/DataReader/DialogueDataReader", order = int.MaxValue)]
public class DialogueDataReader : DataReaderBase
{
    [Header("스프레드 시트에서 불러온 데이터")]
    [SerializeField] public List<DialogueData> DialogueList = new List<DialogueData>();

    public DialogueData GetDialogueByID(int id)
    {
        return DialogueList.Find(dialogue => dialogue.id == id);
    }

    internal void UpdateStats(List<GSTU_Cell> list)
    {
        int id = -1;
        string characterName = null, context = null, spriteName = null, soundName = null, animName = null;
        BGType bGType = BGType.None;
        ActionType actionType = ActionType.None;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "ID":
                    if (!int.TryParse(list[i].value, out id))
                    {
                        if (DialogueList.Count > 0)
                        {
                            id = DialogueList[DialogueList.Count - 1].id;
                        }
                        else return;
                    }
                    break;
                case "Character Name":
                    if (string.IsNullOrWhiteSpace(list[i].value))
                        characterName = "";
                    else
                    {
                        characterName = list[i].value;
                    }
                    break;
                case "Context":
                    context = list[i].value;
                    break;
                case "Sprite name":
                    spriteName = list[i].value;
                    break;
                case "Sound Name":
                    soundName = list[i].value;
                    break;
                case "Anim Name":
                    if (string.IsNullOrWhiteSpace(list[i].value))
                        animName = "Idle";
                    else
                        animName = list[i].value;
                    break;
                case "BGType":
                    if (!string.IsNullOrWhiteSpace(list[i].value))
                        bGType = (BGType)Enum.Parse(typeof(BGType), list[i].value);
                    break;
            }
        }

        DialogueList.Add(new DialogueData(id, characterName, context, spriteName, soundName, animName, bGType, actionType));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueDataReader))]
public class DialogueDataReaderEditor : Editor
{
    DialogueDataReader data;

    void OnEnable()
    {
        data = (DialogueDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 가져오기"))
        {
            UpdateStats(UpdateMethodOne);
            data.DialogueList.Clear();
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; i++)
        {
            data.UpdateStats(ss.rows[i]);
        }

        EditorUtility.SetDirty(target);
    }
}
#endif
