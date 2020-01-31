using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum DialogueType
{
    Normal = 0,
    Option = 1
}

public enum DialogueResult
{
    Positive = 0,
    Neutral = 1,
    Negative = 2
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "GGJ/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    private int currentNode = -1;
    public List<DialogueNode> nodes = new List<DialogueNode>();

    public DialogueNode GetNextNode()
    {
        if (currentNode < nodes.Count)
        {
            currentNode++;
            return nodes[currentNode];
        }
        else
        {
            return null;
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    public DialogueType type = DialogueType.Normal;
    public string text = string.Empty;
    public DialogueOption[] options = new DialogueOption[3];
}

[System.Serializable]
public class DialogueOption
{
    public string emoji;
    public DialogueResult effect = DialogueResult.Neutral;
}
