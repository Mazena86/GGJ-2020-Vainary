using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bubble : MonoBehaviour
{
    private SpriteRenderer sprite;
    private List<Sprite> emojis;
    private int counter;
    private bool animating = false;

    // adjustable values
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Sprite initBubble;
    [SerializeField] private Sprite regularBubble;

    void Start() 
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Initialize(List<Sprite> sprites, Vector3 startPos, float xOffset)
    {
        emojis = sprites;
        counter = emojis.Count;
        gameObject.transform.position = startPos;
    }


}
