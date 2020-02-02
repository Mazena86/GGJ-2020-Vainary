using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
README

This Component will store a copy of its original position and calculates an off-screen position which hides it.
*/

public class Drop : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 outPosition;
    private bool animating = false;
    [SerializeField] AnimationCurve curve;

    void Awake()
    {
        RectTransform got = gameObject.transform as RectTransform;
        originalPosition = got.anchoredPosition;
    }

    public void Close(bool isVerticalMovement, bool isPositiveMovement, float animationTime)
    {
        DropOut(isVerticalMovement, isPositiveMovement, animationTime);
    }

    private void GetOutPosition(bool isVerticalMovement, bool isPositiveMovement)
    {
        if(isVerticalMovement)
        {
            outPosition = new Vector3(originalPosition.x, 
            isPositiveMovement? originalPosition.y + Screen.height : originalPosition.y - Screen.height, 
            originalPosition.z);
        }
        else
        {
            outPosition = new Vector3(isPositiveMovement? originalPosition.x + Screen.width : originalPosition.x - Screen.width, 
            originalPosition.y, 
            originalPosition.z);
        }
    }

    public void DropIn(bool isVerticalMovement, bool isPositiveMovement, float animationTime)
    {
        Debug.Log("Dropping in");
        if(animating)
        {
            return;
        }
        animating = true;
        gameObject.SetActive(true);
        GetOutPosition(isVerticalMovement, isPositiveMovement);

        // begin dropin
        StartCoroutine(DropAnimation(true, animationTime));
    }

    public void DropOut(bool isVerticalMovement, bool isPositiveMovement, float animationTime)
    {
        Debug.Log("'dropping out'");
        if(animating)
        {
            return;
        }
        animating = true;
        GetOutPosition(isVerticalMovement, isPositiveMovement);
        StartCoroutine(DropAnimation(false, animationTime));
    }

    IEnumerator DropAnimation(bool incoming, float animationTime = 0.5f)
    {
        // move the object out-of-screen if necessary
        if(incoming)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = outPosition;
        }

        // lerp the object's position
        float timer = 0;
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            float xPos = Mathf.Lerp(rt.anchoredPosition.x, incoming? originalPosition.x : outPosition.x, curve.Evaluate(timer / animationTime));
            float yPos = Mathf.Lerp(rt.anchoredPosition.y, incoming? 0 : outPosition.y, curve.Evaluate(timer / animationTime));
            // float zPos = Mathf.Lerp(rt.anchoredPosition.z, incoming? originalPosition.z : outPosition.z, curve.Evaluate(timer / animationTime));

            rt.anchoredPosition = new Vector3(xPos, yPos);
            yield return null;
        }

        if(!incoming)
        {
            gameObject.SetActive(false);
        }
        animating = false;
    }
}
