using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetBackground : MonoBehaviour
{
    public CompositeCollider2D world;

    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (world != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            Bounds worldBounds = world.bounds;
            Vector3 pos = world.transform.position + worldBounds.center;
            spriteRenderer.size = worldBounds.size;
            transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
