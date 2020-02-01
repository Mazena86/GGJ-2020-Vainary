using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;

    [SerializeField] float closedAngle;
    [SerializeField] float openAngle;

    bool open = false;
    float animationDuration = 0.75f;

    public void OperateDoor()
    {
        StartCoroutine(OperateDoorRoutine());
    }

    IEnumerator OperateDoorRoutine()
    {
        float timer = 0f;
        float startAngle = transform.rotation.z;
        float targetAngle = open ? closedAngle : openAngle;

        while(timer < animationDuration)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, animationDuration);
            float rotationAngle = Mathf.Lerp(startAngle, targetAngle, curve.Evaluate(timer / animationDuration));
            transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
            yield return null;
        }

        open = !open;
    }

}
