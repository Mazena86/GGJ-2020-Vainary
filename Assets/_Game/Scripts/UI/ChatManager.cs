using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; private set; }

    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private int maxBubbles;
    private List<Bubble> activeBubbles = new List<Bubble>();
    private Queue<Bubble> inactiveBubbles = new Queue<Bubble>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < maxBubbles; i++)
        {
            GameObject newBubble = Instantiate(bubblePrefab, Vector3.zero, Quaternion.identity);
            Bubble script = newBubble.GetComponent<Bubble>();
            inactiveBubbles.Enqueue(script);
        }
    }

    public void AddBubble(List<Sprite> emojis)
    {
        Bubble bubble = inactiveBubbles.Dequeue();
        bubble.Initialize(emojis, Vector3.zero, 0);
        // Move others forward
        foreach(Bubble activeBubble in activeBubbles)
        {
            // activeBubble.MoveUp()
        }
        activeBubbles.Add(bubble);
        // Clear last bubble
        if (activeBubbles.Count >= maxBubbles)
        {
            ClearBubble(activeBubbles[activeBubbles.Count - 1]);
        }
    }

    public void ClearBubble(Bubble bubble)
    {
        // bubble.FadeOut();
        activeBubbles.Remove(bubble);
        inactiveBubbles.Enqueue(bubble);
    }

    public void ClearBubbles()
    {
        foreach(Bubble bubble in activeBubbles)
        {
            ClearBubble(bubble);
        }
    }
}
