using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterController : MonoBehaviour
{
    public static DialogueCharacterController Instance { get; private set; }

    private Dictionary<string, Transform> _registeredCharacters = new Dictionary<string, Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterCharacter(string characterName, Transform characterTransform)
    {
        if (!_registeredCharacters.ContainsKey(characterName))
        {
            _registeredCharacters.Add(characterName, characterTransform);
        }
        else
        {
            _registeredCharacters[characterName] = characterTransform;
        }
    }

    public void UnregisterCharacter(string characterName)
    {
        if (_registeredCharacters.ContainsKey(characterName))
        {
            _registeredCharacters.Remove(characterName);
        }
    }

    public Transform GetCharacterTransform(string characterName)
    {
        if (_registeredCharacters.TryGetValue(characterName, out Transform characterTransform))
        {
            return characterTransform;
        }
        return null;
    }
}
