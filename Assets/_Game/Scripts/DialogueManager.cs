﻿using System;
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

    public int Score { get; private set; } = 0;

    public GameObject Patient { get; private set; }

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
        endScreen.SetActive(false);
        HideOptions();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNode = 0;
    }

    public void SpawnPatient(GameObject patientPrefab)
    {
        Patient = Instantiate(patientPrefab, Vector3.zero, Quaternion.identity);
    }

    public void PlayDialogue()
    {
        DialogueNode node = currentDialogue.GetNode(currentNode);
        currentNode++;
        if (node == null)
        {
            Debug.Log("End");
            endScreen.SetActive(true);
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
        Score += 2;
        Debug.Log("Positive");
        OnOptionSelected();
    }

    public void DoNeutralEffect()
    {
        Score++;
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
