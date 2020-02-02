using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Bubble : MonoBehaviour
{
    private List<Sprite> emojis;
    private int counter;
    private bool animating = false;

    // adjustable values
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Image initBubble;
    [SerializeField] private Image regularBubble;
    [SerializeField] private Vector3 initEmojiPosition;

    public void Initialize(List<Sprite> sprites, Vector3 startPos, float xOffset)
    {
        emojis = sprites;
        counter = emojis.Count;
        gameObject.GetComponent<RectTransform>().anchoredPosition = startPos;
        foreach(Transform child in transform)
        {
            child.GetComponent<Image>().sprite = null;
            child.GetComponent<Image>().enabled = false;
        }
        FadeIn();
    }

    public IEnumerator TypewriteEmojis()
    {
        Queue<Sprite> emojiQueue = new Queue<Sprite>(emojis);
        int index = 0;
        while (emojiQueue.Count > 0)
        {
            Sprite next = emojiQueue.Dequeue();
            GameObject slot = gameObject.transform.GetChild(index).gameObject;
            slot.GetComponent<Image>().sprite = next;
            slot.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            index++;
        }
        yield return new WaitForSeconds(.1f);
        // Done writing so request next bubble
        DialogueManager.Instance.PlayDialogue();
    }

    public void MoveUp(float xOffset)
    {
        StartCoroutine(MoveAnimation(xOffset));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeAnimation(true));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAnimation(false));
    }

    IEnumerator FadeAnimation(bool fadein)
    {
        // setup
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 origScale = new Vector3(1, 1, 1);
        if(fadein)
        {
            // make it invisibly small
            rectTransform.localScale = new Vector3(0,0,0);
        }
        float scalingFactor = 0;
        float timer = 0;
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            if(fadein)
            {
                // TODO make this animationcurve
                scalingFactor = Mathf.Lerp(0, origScale.y, timer / animationTime);
            }
            else
            {
                scalingFactor = Mathf.Lerp(origScale.y, 0, timer / animationTime);
            }
            rectTransform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
            yield return null;
        }
        if(!fadein)
        {
            // The object should be moved to unused list
            gameObject.SetActive(false);
            Debug.Log("Done FadeOut");
        }
        else
        {
            StartCoroutine(TypewriteEmojis());
            Debug.Log("Done FadeIn");
        }
    }

    IEnumerator MoveAnimation(float yOffset)
    {
        // setup
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        float startY = rectTransform.anchoredPosition.y;
        float endY = rectTransform.anchoredPosition.y + yOffset;
        Debug.Log("Going to position y " + endY);

        // lerp the object's position
        float timer = 0;
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            float yPos = Mathf.Lerp(startY, endY, timer / animationTime);

            rectTransform.anchoredPosition = new Vector3(0, yPos, 0);

            yield return null;
        }
    }
}
