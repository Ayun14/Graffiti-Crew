using System;
using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "FriendDataSO", menuName = "SO/UI/Chapter")]
    [Serializable]
    public class MemberDataSO : ScriptableObject // ũ���
    {
        public string name;
        public string ability;
        public Sprite profile;
        [Space]
        public bool isMyCrew = false;

    }
}