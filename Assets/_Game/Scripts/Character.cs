using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    GameObject spawnPoint;
    GameObject sofaPoint;
    DoorScript door;

    NavMeshAgent agent;
    
    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
        sofaPoint = GameObject.Find("SofaPoint");
        door = FindObjectOfType<DoorScript>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(agent.destination == spawnPoint.transform.position && agent.remainingDistance < .3f)
        {
            agent.isStopped = true;
            door.OperateDoor();
        }
        if(agent.destination == sofaPoint.transform.position && agent.remainingDistance < .2f)
        {
            agent.isStopped = true;
            // Jump on sofa
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(sofaPoint.transform.position);
        door.OperateDoor(0.75f);
    }

    public void Leave()
    {
        agent.SetDestination(spawnPoint.transform.position);
    }
}
