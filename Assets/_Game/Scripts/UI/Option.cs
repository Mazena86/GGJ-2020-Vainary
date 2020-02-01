using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Option : MonoBehaviour
{
    private DialogueResult result;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Initialize(List<Sprite> sprites, DialogueResult result)
    {
        this.result = result;
        int index = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().sprite = sprites[index];
            index++;
        }
    }

    public void OnClick()
    {
        switch(result)
        {
            case DialogueResult.Positive:
                DialogueManager.Instance.DoPositiveEffect();
                break;

            case DialogueResult.Neutral:
                DialogueManager.Instance.DoNeutralEffect();
                break;

            case DialogueResult.Negative:
                DialogueManager.Instance.DoNegativeEffect();
                break;
        }
    }
}
