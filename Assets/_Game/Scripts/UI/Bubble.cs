using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Image))]
public class Bubble : MonoBehaviour
{
    private List<Image> emojis;
    private int counter;
    private bool animating = false;

    // adjustable values
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Image initBubble;
    [SerializeField] private Image regularBubble;
    [SerializeField] private Vector3 initEmojiPosition;

    public void Initialize(List<Image> sprites, Vector3 startPos, float xOffset)
    {
        emojis = sprites;
        counter = emojis.Count;
        gameObject.transform.position = startPos;
        FadeIn();
    }

    private void TypewriteEmojis()
    {

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
            Debug.Log("Done FadeOut");
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
