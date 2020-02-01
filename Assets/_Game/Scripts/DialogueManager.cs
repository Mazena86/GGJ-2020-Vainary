using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public Dialogue currentDialogue;
    [SerializeField] private GameObject[] options;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private List<CodeEmojiPair> emojis;
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

    public void PlayDialogue()
    {
        DialogueNode node = currentDialogue.GetNextNode();
        if (node == null)
        {
            // Show end screen
        }
        if (node.type == DialogueType.Normal)
        {
            ShowText(node);
        }
        else if (node.type == DialogueType.Option)
        {
            ShowOptions(node);
        }
    }

    public void ShowText(DialogueNode node)
    {
        List<Sprite> nodeEmojis = ParseEmojis(node.text);
        ChatManager.Instance.AddBubble(nodeEmojis);
    }

    private List<Sprite> ParseEmojis(string text)
    {
        List<Sprite> parsed = new List<Sprite>();
        string[] codes = text.Split(' ');
        for (int i = 0; i < codes.Length; i++)
        {
            CodeEmojiPair emoji = emojis.Find(x => x.code == codes[i]);
            if (emoji != null)
            {
                parsed.Add(emoji.sprite);
            }
            else
            {
                Debug.LogError("No matching emoji for code " + codes[i]);
            }
        }
        return parsed;
    }

    public void ShowOptions(DialogueNode node)
    {
        for (int i = 0; i < node.options.Length; i++)
        {
            options[i].SetActive(true);
        }
    }

    public void HideOptions()
    {
        foreach(GameObject option in options)
        {
            option.SetActive(false);
        }
    }

    public void DoPositiveEffect()
    {
        score += 2;
        OnOptionSelected();
    }

    public void DoNeutralEffect()
    {
        score++;
        OnOptionSelected();
    }

    public void DoNegativeEffect()
    {
        OnOptionSelected();
    }

    public void OnOptionSelected()
    {
        HideOptions();
        PlayDialogue();
    }
}

[Serializable]
public class CodeEmojiPair
{
    public string code;
    public Sprite sprite;
}
