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
        gameObject.transform.position = startPos;
        foreach(Transform child in transform)
        {
            child.GetComponent<Image>().sprite = null;
        }
        FadeIn();
    }

    public IEnumerator TypewriteEmojis()
    {
        Stack<Sprite> emojiQueue = new Stack<Sprite>(emojis);
        int index = 0;
        while (emojiQueue.Count > 0)
        {
            Sprite next = emojiQueue.Pop();
            GameObject slot = gameObject.transform.GetChild(index).gameObject;
            slot.GetComponent<Image>().sprite = next;
            yield return new WaitForSeconds(1);
            index++;
        }
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
        Vector3 origScale = transform.localScale; // should be (1,1,1)
        if(fadein)
        {
            // make it invisibly small
            transform.localScale = new Vector3(0,0,0);
        }
        float scalingFactor = 0;
        float timer = 0;
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            if(fadein)
            {
                // TODO make this animationcurve
                scalingFactor = Mathf.Lerp(0, origScale.x, timer / animationTime);
            }
            else
            {
                scalingFactor = Mathf.Lerp(origScale.x, 0, timer / animationTime);
            }
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

    IEnumerator MoveAnimation(float xOffset)
    {
        // setup
        Vector3 outPosition = transform.position + new Vector3(xOffset, 0, 0);
        Debug.Log("Going to position " + outPosition);

        // lerp the object's position
        float timer = 0;
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            float xPos = Mathf.Lerp(gameObject.transform.position.x, outPosition.x, timer / animationTime);

            gameObject.transform.position += new Vector3(xPos, 0, 0);

            yield return null;
        }
    }
}
