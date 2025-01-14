using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

public class DialogueUIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Image characterImage;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueDataReader dialogueDataReader;

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private int currentDialogueIndex = 0;
    private int dialogueStartID = 0;
    private int dialogueEndID = 0;

    private Action onDialogueComplete;

    private List<DialogueData> filteredDialogueList;

    private void Start()
    {
        dialogueUI.SetActive(false);
        nameText.text = "";
        contextText.text = "";
        characterImage.sprite = null;
    }

    public void StartDialogue(int startID, int endID, Action onComplete)
    {
        if (dialogueDataReader == null || dialogueDataReader.DialogueList.Count == 0)
        {
            Debug.LogError("DialogueDataReader가 설정되지 않았거나 데이터가 비어 있습니다.");
            onComplete?.Invoke();
            return;
        }

        dialogueStartID = startID;
        dialogueEndID = endID;

        filteredDialogueList = dialogueDataReader.DialogueList.FindAll(dialogue =>
            dialogue.id >= dialogueStartID && dialogue.id <= dialogueEndID);

        if (filteredDialogueList.Count == 0)
        {
            Debug.LogWarning("지정된 ID 범위에 대화 데이터가 없습니다.");
            onComplete?.Invoke();
            return;
        }

        dialogueUI.SetActive(true);
        currentDialogueIndex = 0;
        onDialogueComplete = onComplete;

        ShowDialogue(currentDialogueIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
                CompleteTyping();
            else
                ShowNextDialogue();
        }
    }

    private void ShowDialogue(int index)
    {
        if (index < 0 || index >= filteredDialogueList.Count)
        {
            Debug.LogWarning("대화 인덱스가 범위를 벗어났습니다.");
            return;
        }

        DialogueData dialogue = filteredDialogueList[index];
        nameText.text = dialogue.characterName;

        Sprite sprite = Resources.Load<Sprite>($"Sprite/{dialogue.spriteName}");
        if (sprite != null)
            characterImage.sprite = sprite;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypingEffect(dialogue.context));
    }

    private void ShowNextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex >= filteredDialogueList.Count)
        {
            dialogueUI.SetActive(false);
            onDialogueComplete?.Invoke();
            return;
        }

        ShowDialogue(currentDialogueIndex);
    }

    private IEnumerator TypingEffect(string fullText)
    {
        isTyping = true;
        contextText.text = "";

        foreach (char letter in fullText)
        {
            contextText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        contextText.text = filteredDialogueList[currentDialogueIndex].context;
        isTyping = false;
    }
}
