using AH.SaveSystem;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter_Stage_", menuName = "SO/Save/Data/StageSaveDataSO")]
public class StageSaveDataSO : SaveDataSO {
    public bool isClear;
    public int star;

    private bool _defaultIsClearData = false;
    private int _defaultStarData = 0;

    private void Awake() {
        _defaultIsClearData = isClear;
        _defaultStarData = star;
    }
    public override string GetDataType() {
        return dataType.ToString();
    }
    public override string GetData() {
        return $"{isClear} {star}";
    }
    public override void SetValueFromString(string value) {
        string[] datas = value.Split(" ");
        isClear = bool.Parse(datas[0]);
        star = int.Parse(datas[1]);
    }
    public override void ResetData() {
        isClear = _defaultIsClearData;
        star = _defaultStarData;
    }
}
