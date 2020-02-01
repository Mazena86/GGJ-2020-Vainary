using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private int currentWaypoint = 0;
    private Vector3 targetLocation = Vector3.zero;
    private GameObject waypoints;

    private void Start()
    {
        waypoints = GameObject.Find("Waypoints");
        targetLocation = waypoints.transform.GetChild(0).transform.position;
    }

    private void Update()
    {
        if (targetLocation != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, targetLocation, speed);
            if (Vector3.Distance(transform.position, targetLocation) < 1)
            {
                currentWaypoint++;
                if (currentWaypoint < 2)
                {
                    targetLocation = waypoints.transform.GetChild(currentWaypoint).transform.position;
                }
                else
                {
                    // TODO: perform lay animation and start dialogue gameObject.GetComponent<Animator>().SetTrigger("")
                }
            }
        }
    }
}
