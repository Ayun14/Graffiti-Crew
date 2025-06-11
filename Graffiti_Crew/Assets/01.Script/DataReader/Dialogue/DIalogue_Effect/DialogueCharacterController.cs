using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterController : MonoBehaviour
{
    public static DialogueCharacterController Instance { get; private set; }

    private Dictionary<string, GameObject> _registeredCharacters = new Dictionary<string, GameObject>();

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

    public void RegisterCharacter(string characterName, GameObject character)
    {
        if (!_registeredCharacters.ContainsKey(characterName))
        {
            _registeredCharacters.Add(characterName, character);
        }
        else
        {
            _registeredCharacters[characterName] = character;
        }
    }

    public void UnregisterCharacter(string characterName)
    {
        if (_registeredCharacters.ContainsKey(characterName))
        {
            _registeredCharacters.Remove(characterName);
        }
    }

    public GameObject GetCharacterGameObject(string characterName)
    {
        if (_registeredCharacters.TryGetValue(characterName, out GameObject character))
        {
            return character;
        }
        return null;
    }
}
