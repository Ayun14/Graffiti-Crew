using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "SO/HangOut/PlayerData")]
public class HangOutPlayerDataSO : ScriptableObject
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
}
