using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dialogue currentDialogue;
    public GameObject dialogueBubblePrefab;
    public List<GameObject> dialogueBubbles;
    public GameObject[] options;
    public GameObject endScreen;
    private int score = 3;

    private void Start()
    {
        StartCoroutine(DialogLoop());
    }

    public void ContinueDialog()
    {
        StartCoroutine(DialogLoop());
    }

    public IEnumerator DialogLoop()
    {
        while (true)
        {
            DialogueNode node = currentDialogue.GetNextNode();
            if (node == null)
            {
                // Get next customer or show end screen
            }
            if (node.type == DialogueType.Normal)
            {
                ShowText(node);
                yield return new WaitForSeconds(2);
            }
            else if (node.type == DialogueType.Option)
            {
                ShowOptions(node);
                break;
            }
        }
    }

    public void ShowText(DialogueNode node)
    {
        GameObject bubble = Instantiate(dialogueBubblePrefab, Vector3.zero, Quaternion.identity);
        dialogueBubbles.Add(bubble);
        // TODO: advance all dialogues;
        // Allow max 3 dialogues and destroy last if more exist
        if (dialogueBubbles.Count > 3)
        {
            Destroy(dialogueBubbles[dialogueBubbles.Count]);
            dialogueBubbles.RemoveAt(dialogueBubbles.Count);
        }
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
        score++;
        OnOptionSelected();
    }

    public void DoNeutralEffect()
    {
        OnOptionSelected();
    }

    public void DoNegativeEffect()
    {
        score--;
        OnOptionSelected();
    }

    public void OnOptionSelected()
    {
        HideOptions();
        ContinueDialog();
    }
}
