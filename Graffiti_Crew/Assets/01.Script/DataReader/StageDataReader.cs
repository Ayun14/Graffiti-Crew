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
    public string title;
    public string description;

    public StageData(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}

[CreateAssetMenu(fileName = "StageReader", menuName = "SO/Stage/StageDataReader", order = int.MaxValue)]
public class StageDataReader : DataReaderBase
{
    [Header("스프레드 시트에서 불러온 데이터")]
    [SerializeField] public List<StageData> StageList = new List<StageData>();

    internal void UpdateStats(List<GSTU_Cell> list)
    {
        string title = null, description = null;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "Title":
                    title = list[i].value;
                    break;
                case "Description":
                    description = list[i].value;
                    break;
            }
        }

        StageList.Add(new StageData(title, description));
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
