using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    GameObject spawnPoint;
    GameObject sofaPoint;
    GameObject sitPosition;
    DoorScript door;
    Animator animator;
    [SerializeField] AnimationCurve jumpTurnCurve;
    GameObject currentDestination = null;
    // Vector3 sitPosition = new Vector3(-180, 0, 14.4f);

    NavMeshAgent agent;
    bool sitting = false;
    bool leaving = false;
    
    private void Awake()
    {
        spawnPoint = GameObject.Find("SpawnPoint");
        sofaPoint = GameObject.Find("SofaPoint");
        sitPosition = GameObject.Find("SitPosition");
        door = FindObjectOfType<DoorScript>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Debug.Log("Target: " + (agent.destination == null ? "null" : agent.destination.ToString()));
        // Debug.Log("Agent stopped: " + agent.isStopped);
        // Debug.Log("Agent distance: " + agent.remainingDistance);
        if(agent.enabled && agent.remainingDistance < .5f && currentDestination == spawnPoint)
        {
            agent.isStopped = true;
            door.OperateDoor();
            currentDestination = null;
            DialogueManager.Instance.ShowEndScreen();
            Destroy(gameObject, 1f);
        }
        if(agent.enabled && agent.remainingDistance < .5f && currentDestination == sofaPoint)
        {
            Debug.Log("Reached sofa");
            currentDestination = null;
            agent.isStopped = true;
            agent.enabled = false;
            StartCoroutine(JumpOnSofa());
            // Jump on sofa
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        leaving = false;
        door.OperateDoor(0.75f);
        StartCoroutine(StartMoving(0.65f));
    }

    IEnumerator StartMoving(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        agent.SetDestination(sofaPoint.transform.position);
        currentDestination = sofaPoint;
    }

    private IEnumerator JumpOnSofa()
    {
        animator.SetTrigger("Jump");
        float maxTime = 1f;
        float timer = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = sitPosition.transform.rotation;
        Vector3 targetPosition = sitPosition.transform.position;
        Vector3 startPosition = transform.position;
        while(timer < maxTime)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, maxTime);
            //float newRotation = Mathf.Lerp(startRotation, targetRotation, jumpTurnCurve.Evaluate(timer / maxTime));
            //transform.rotation = Quaternion.Euler(0, newRotation, 0);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, jumpTurnCurve.Evaluate(timer / maxTime));
            transform.position = Vector3.Lerp(startPosition, targetPosition, jumpTurnCurve.Evaluate(timer / maxTime));
            Debug.Log("Turning");
            yield return null;
        }
        sitting = true;
    }

    public void PatientOnSofa()
    {
        Debug.Log("On sofa");
        if(!DialogueManager.Instance.currentDialoguePlayed)
        {
            StartCoroutine(OnSofaRoutine());
        }
    }

    private IEnumerator OnSofaRoutine()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.Instance.PlayDialogue();
    }

    public void Leave()
    {
        leaving = true;
        animator.SetTrigger("Jump");
    }

    public void StoodUp()
    {
        if(leaving)
        {
            Debug.Log("Leaving");
            StartCoroutine(LeaveRoutine());
            leaving = false;
        }
    }

    private IEnumerator LeaveRoutine()
    {
        sitting = false;
        
        float timer = 0;
        float timeToMove = 1f;
        Vector3 startPos = transform.position;
        while(timer < timeToMove)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, timeToMove);
            transform.position = Vector3.Lerp(startPos, sofaPoint.transform.position, timer / timeToMove);
            yield return null;
        }
        agent.enabled = true;
        agent.ResetPath();
        agent.SetDestination(spawnPoint.transform.position);
        agent.isStopped = false;
        currentDestination = spawnPoint;
    }

    //private IEnumerator MoveTo(Vector3 target)
    //{

    //}
}
