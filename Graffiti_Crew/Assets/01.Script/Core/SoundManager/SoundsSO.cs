using UnityEngine;

namespace SmallHedge.SoundManager
{
    [CreateAssetMenu(menuName = "SO/SoundSO")]
    public class SoundsSO : ScriptableObject
    {
        public SoundList[] sounds;
    }
}