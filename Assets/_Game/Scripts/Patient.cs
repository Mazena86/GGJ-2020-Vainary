using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPatient", menuName = "GGJ/Patient", order = 1)]
public class Patient : ScriptableObject
{
    public GameObject prefab;
    public Dialogue dialogue;
}
