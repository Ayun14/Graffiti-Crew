using UnityEngine;

[CreateAssetMenu(fileName = "StageDescriptionSO", menuName = "SO/UI/Stage/StageDescriptionSO")]
public class StageDescriptionSO : ScriptableObject
{
    public string title;
    public string description;
    public Sprite graffiti;

    public AdmissionTicket[] ticket;
}
