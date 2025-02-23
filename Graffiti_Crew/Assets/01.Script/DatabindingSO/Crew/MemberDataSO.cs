using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AH.UI.Data {
    [CreateAssetMenu(fileName = "FriendDataSO", menuName = "SO/UI/Crew/Chapter")]
    [Serializable]
    public class MemberDataSO : ScriptableObject // Å©·ç¿ø
    {
        [Header("Dialogue")]
        public string memberName;
        public string ability;
        public Sprite profile;

        [Header("Bool")]
        public bool isMyCrew = false;

    }
}