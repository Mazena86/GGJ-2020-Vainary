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
    public float animationTime = 0.5f;
    private bool animating = false;

    void Awake()
    {
        originalPosition = gameObject.transform.position;
    }

    public void Close(bool isVerticalMovement, bool isPositiveMovement)
    {
        DropOut(isVerticalMovement, isPositiveMovement);
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

    public void DropIn(bool isVerticalMovement, bool isPositiveMovement, int delay = 0)
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
        StartCoroutine(DropAnimation(true, delay));
    }

    public void DropOut(bool isVerticalMovement, bool isPositiveMovement, int delay = 0)
    {
        Debug.Log("'dropping out'");
        if(animating)
        {
            return;
        }
        animating = true;
        GetOutPosition(isVerticalMovement, isPositiveMovement);
        StartCoroutine(DropAnimation(false, delay));
    }

    IEnumerator DropAnimation(bool incoming, int delay = 0)
    {
        // move the object out-of-screen if necessary
        if(incoming)
        {
            gameObject.transform.position = outPosition;
        }

        // wait for it... 
        yield return new WaitForSecondsRealtime(delay);

        // lerp the object's position
        float timer = 0;
        while(timer < animationTime)
        {
            timer = Mathf.Clamp(timer + Time.unscaledDeltaTime, 0, animationTime);

            float xPos = Mathf.Lerp(gameObject.transform.position.x, incoming? originalPosition.x : outPosition.x, timer / animationTime);
            float yPos = Mathf.Lerp(gameObject.transform.position.y, incoming? originalPosition.y : outPosition.y, timer / animationTime);
            float zPos = Mathf.Lerp(gameObject.transform.position.z, incoming? originalPosition.z : outPosition.z, timer / animationTime);

            gameObject.transform.position = new Vector3(xPos, yPos, zPos);
            yield return null;
        }

        if(!incoming)
        {
            gameObject.SetActive(false);
        }
    }
}
