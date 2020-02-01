using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public Dialogue currentDialogue;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject endScreen;
    private int currentNode = 0;
    private int score = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideOptions();
        PlayDialogue();
    }

    public void PlayDialogue()
    {
        DialogueNode node = currentDialogue.GetNode(currentNode);
        currentNode++;
        if (node == null)
        {
            Debug.Log("End");
            // Show end screen
        }
        else if (node.type == DialogueType.Normal)
        {
            Debug.Log("Show text");
            ShowText(node);
        }
        else if (node.type == DialogueType.Option)
        {
            Debug.Log("Show options");
            ShowOptions(node);
        }
    }

    public void ShowText(DialogueNode node)
    {
        ChatManager.Instance.AddBubble(node.emojis);
    }

    public void ShowOptions(DialogueNode node)
    {
        options.SetActive(true);
        int index = 0;
        foreach(Transform option in options.transform)
        {
            option.gameObject.GetComponent<Option>().Initialize(node.options[index].emojis, node.options[index].effect);
            index++;
        }
    }

    public void HideOptions()
    {
        options.SetActive(false);
    }

    public void DoPositiveEffect()
    {
        score += 2;
        Debug.Log("Positive");
        OnOptionSelected();
    }

    public void DoNeutralEffect()
    {
        score++;
        Debug.Log("Neutral");
        OnOptionSelected();
    }

    public void DoNegativeEffect()
    {
        Debug.Log("Negative");
        OnOptionSelected();
    }

    public void OnOptionSelected()
    {
        HideOptions();
        PlayDialogue();
    }
}
