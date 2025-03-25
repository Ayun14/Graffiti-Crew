using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryDialogueSO", menuName = "SO/StoryDialogueSO")]
public class StoryDialogueSO : ScriptableObject
{
    public List<NPCSO> storyList;
}
