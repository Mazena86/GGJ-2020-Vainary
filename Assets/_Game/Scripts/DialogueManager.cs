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
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private ParticleSystem angryParticles;
    [SerializeField] private ParticleSystem happyParticles;
    private int currentNode = 0;

    public int Score { get; private set; } = 0;

    public GameObject Patient { get; private set; }

    public Sprite PatientPicture { get; private set; }

    public bool currentDialoguePlayed = false;

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
        Score = 0;
        currentDialoguePlayed = false;
        Debug.Log("Dialogue called: " + dialogue.name);
    }

    public void SetPatientPicture(Sprite picture)
    {
        PatientPicture = picture;
    }

    public void SpawnPatient(GameObject patientPrefab)
    {
        Patient = Instantiate(patientPrefab, spawnPoint.transform.position, Quaternion.identity);
        Patient.name = patientPrefab.name;
        // Patient.GetComponent<Animator>().SetTrigger("Enter");
    }

    public void ShowEndScreen()
    {
        endScreen.SetActive(true);
    }

    public void PlayDialogue()
    {
        Debug.Log("Called PlayDialogue");
        DialogueNode node = currentDialogue.GetNode(currentNode);
        currentNode++;
        if (node == null)
        {
            Debug.Log("End");
            ChatManager.Instance.ClearBubbles();
            // endScreen.SetActive(true);
            Patient.GetComponent<Character>().Leave();
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
        currentDialoguePlayed = true;
    }

    public void ShowText(DialogueNode node)
    {
        AudioManager.Instance.Play(Patient.name + "Mumble");
        if(!ChatManager.Instance.ChatCanvas.activeInHierarchy)
        {
            ChatManager.Instance.ChatCanvas.SetActive(true);
        }
        ChatManager.Instance.AddBubble(node.emojis);
    }

    public void ShowOptions(DialogueNode node)
    {
        AudioManager.Instance.Stop(Patient.name + "Mumble");
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
        happyParticles.Play();
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
        angryParticles.Play();
        Debug.Log("Negative");
        OnOptionSelected();
    }

    public void OnOptionSelected()
    {
        AudioManager.Instance.Play("Click");
        HideOptions();
        PlayDialogue();
    }
}
