using UnityEngine;

public class DialogueCharacter : MonoBehaviour
{
    public string characterName;

    private void OnEnable()
    {
        if (DialogueCharacterController.Instance != null)
        {
            DialogueCharacterController.Instance.RegisterCharacter(characterName, gameObject);
        }
    }

    private void OnDisable()
    {
        if (DialogueCharacterController.Instance != null)
        {
            DialogueCharacterController.Instance.UnregisterCharacter(characterName);
        }
    }
}