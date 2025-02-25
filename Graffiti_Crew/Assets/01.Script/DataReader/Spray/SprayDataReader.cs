using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct SprayData
{
    public string root;
    public string num;

    public SprayData(string root, string num)
    {
        this.root = root;
        this.num = num;
    }
}

[CreateAssetMenu(fileName = "SprayReader", menuName = "SO/DataReader/SprayDataReader", order = int.MaxValue)]
public class SprayDataReader : DataReaderBase
{
    [Header("스프레드 시트에서 불러온 데이터")]
    [SerializeField] public List<SprayData> SprayList = new List<SprayData>();

    internal void UpdateStats(List<GSTU_Cell> list)
    {
        string root = null, num = null;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "Root":
                    root = list[i].value;
                    break;
                case "Num":
                    num = list[i].value;
                    break;
            }
        }

        SprayList.Add(new SprayData(root, num));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SprayDataReader))]
public class SprayDataReaderEditor : Editor
{
    SprayDataReader data;

    void OnEnable()
    {
        data = (SprayDataReader)target;
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
