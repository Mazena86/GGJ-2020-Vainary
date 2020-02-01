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
    public List<DialogueNode> nodes = new List<DialogueNode>();

    public DialogueNode GetNode(int index)
    {
        return index < nodes.Count ? nodes[index] : null;
    }
}

[System.Serializable]
public class DialogueNode
{
    public DialogueType type = DialogueType.Normal;
    public List<Sprite> emojis = new List<Sprite>();
    public DialogueOption[] options = new DialogueOption[3];
}

[System.Serializable]
public class DialogueOption
{
    public List<Sprite> emojis;
    public DialogueResult effect = DialogueResult.Neutral;
}
