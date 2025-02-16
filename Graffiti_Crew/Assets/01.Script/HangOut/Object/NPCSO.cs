using UnityEngine;

[CreateAssetMenu(fileName = "NPCSO", menuName = "SO/NPC/NPCData")]
public class NPCSO : ScriptableObject
{
    public StageSaveDataSO lastStageDataSO;
    public int startIndex;
    public int endIndex;
}
