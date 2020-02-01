using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            for (int i = 0; i < maxBubbles; i++)
            {
                GameObject newBubble = Instantiate(bubblePrefab, Vector3.zero, Quaternion.identity);
                newBubble.SetActive(false);
                newBubble.transform.SetParent(transform, true);
                Bubble script = newBubble.GetComponent<Bubble>();
                inactiveBubbles.Enqueue(script);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBubble(List<Sprite> emojis)
    {
        Bubble bubble = inactiveBubbles.Dequeue();
        bubble.gameObject.SetActive(true);
        bubble.Initialize(emojis, Vector3.zero, 0);
        // Move others forward
        foreach(Bubble activeBubble in activeBubbles)
        {
            activeBubble.MoveUp(50);
        }
        activeBubbles.Add(bubble);
        // Clear last bubble
        if (activeBubbles.Count >= maxBubbles)
        {
            ClearBubble(activeBubbles[0]);
        }
    }

    public void ClearBubble(Bubble bubble)
    {
        bubble.FadeOut();
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
