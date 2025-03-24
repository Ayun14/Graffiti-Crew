using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct StageData
{
    public string id;
    public string title;
    public string description;
    public string sprite;

    public StageData(string id, string title, string description, string sprite)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.sprite = sprite;
    }
}

[CreateAssetMenu(fileName = "StageReader", menuName = "SO/DataReader/StageDataReader", order = int.MaxValue)]
public class StageDataReader : DataReaderBase
{
    [Header("스프레드 시트에서 불러온 데이터")]
    [SerializeField] public List<StageData> StageList = new List<StageData>();

    internal void UpdateStats(List<GSTU_Cell> list)
    {
        string id = null, title = null, description = null, sprite = null;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "ID":
                    id = list[i].value;
                    break;
                case "Title":
                    title = list[i].value;
                    break;
                case "Description":
                    description = list[i].value;
                    break;
                case "Sprite":
                    sprite = list[i].value;
                    break;
            }
        }

        StageList.Add(new StageData(id, title, description, sprite));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(StageDataReader))]
public class StageDataReaderEditor : Editor
{
    StageDataReader data;

    void OnEnable()
    {
        data = (StageDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 가져오기"))
        {
            UpdateStats(UpdateMethodOne);
            data.StageList.Clear();
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
