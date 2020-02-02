using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    GameObject spawnPoint;
    GameObject sofaPoint;
    DoorScript door;
    Animator animator;
    [SerializeField] AnimationCurve jumpTurnCurve;

    NavMeshAgent agent;
    
    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
        sofaPoint = GameObject.Find("SofaPoint");
        door = FindObjectOfType<DoorScript>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    private IEnumerator JumpOnSofa()
    {
        animator.SetTrigger("Jump");
        float maxTime = .3f;
        float timer = 0;
        float startRotation = transform.rotation.y;
        float targetRotation = 180;
        while(timer < maxTime)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, maxTime);
            float newRotation = Mathf.Lerp(startRotation, targetRotation, jumpTurnCurve.Evaluate(timer / maxTime));
            transform.rotation = Quaternion.Euler(0, newRotation, 0);
            yield return null;
        }
    }

    public void PatientOnSofa()
    {

    }

    public void Leave()
    {
        agent.SetDestination(spawnPoint.transform.position);
    }
}
