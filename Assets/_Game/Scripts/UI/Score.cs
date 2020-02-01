using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private string id;

    private void OnEnable()
    {
        int score = PlayerPrefs.GetInt(id, 0);
        int index = 0;
        foreach(Transform child in transform)
        {
            child.GetChild(0).gameObject.SetActive(index < score);
            index++;
        }
    }
}
