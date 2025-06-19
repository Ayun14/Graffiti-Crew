using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using AH.UI.Data;


#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct TMIData
{
    public string id;
    public string tmi;

    public TMIData(string id, string tmi)
    {
        this.id = id;
        this.tmi = tmi;
    }
}

[CreateAssetMenu(fileName = "TMIReader", menuName = "SO/DataReader/TMIDataReader", order = int.MaxValue)]
public class TMIDataReader : DataReaderBase
{
    [Header("스프레드 시트에서 불러온 데이터")]
    [SerializeField] public List<TMIData> SprayList = new List<TMIData>();
    internal void UpdateStats(List<GSTU_Cell> list)
    {
        string id = null, tmi = null;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "ID":
                    id = list[i].value;
                    break;
                case "TMI":
                    tmi = list[i].value;
                    break;
            }
        }

        SprayList.Add(new TMIData(id, tmi));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TMIDataReader))]
public class SprayDataReaderEditor : Editor
{
    TMIDataReader data;

    void OnEnable()
    {
        data = (TMIDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 가져오기"))
        {
            UpdateStats(UpdateMethodOne);
            data.SprayList.Clear();
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
