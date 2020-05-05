using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetBackground : MonoBehaviour
{
    public CompositeCollider2D world;
    PolygonCollider2D polyCollider;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (world != null)
        {
            polyCollider = GetComponent<PolygonCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            Bounds worldBounds = world.bounds;
            Vector3 pos = world.transform.position + worldBounds.center;
            spriteRenderer.size = worldBounds.size;
            transform.position = pos;
            polyCollider.points = BoundsToPoints(worldBounds, transform.position);
        }
    }

    Vector2[] BoundsToPoints(Bounds bounds,Vector3 offset)
    {
        Vector2[] arr = new Vector2[4];
        Vector2 newOffset = new Vector2(offset.x, offset.y);
        arr[0] = new Vector2(bounds.min.x, bounds.min.y + bounds.size.y) - newOffset;
        arr[1] = bounds.min - offset;
        arr[2] = new Vector2(bounds.min.x + bounds.size.x, bounds.min.y) - newOffset;
        arr[3] = bounds.max - offset;
        return arr;
    }
}
