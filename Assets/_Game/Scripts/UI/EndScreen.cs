﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject scores;
    [SerializeField] private Image picture;

    private void OnEnable()
    {
        int calculatedScore = 0;
        switch(DialogueManager.Instance.Score)
        {
            case 0:
                calculatedScore = 0;
                break;
            case 1:
                calculatedScore = 1;
                break;
            case 2:
                calculatedScore = 2;
                break;
            case 3:
            case 4:
                calculatedScore = 3;
                break;
            case 5:
                calculatedScore = 4;
                break;
            case 6:
                calculatedScore = 5;
                break;
        }
        for (int i = 0; i < scores.transform.childCount; i++)
        {
            scores.transform.GetChild(i).GetChild(0).gameObject.SetActive(i < calculatedScore);
        }
        picture.sprite = DialogueManager.Instance.PatientPicture;
        if (calculatedScore > PlayerPrefs.GetInt(DialogueManager.Instance.Patient.name, 0))
        {
            PlayerPrefs.SetInt(DialogueManager.Instance.Patient.name, calculatedScore);
            Debug.Log("Saved score for " + DialogueManager.Instance.Patient.name);
        }
    }
}
