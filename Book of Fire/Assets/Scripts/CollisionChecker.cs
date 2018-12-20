using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
    [HideInInspector] public CapsuleCollider2D col;
    public LayerMask collisionMask;
    public float verticalOffset;

    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    public bool CheckVertical(float movementY)
    {
        if (movementY == 0)
            //rayLength = 0,1; directionY = -1;
            return true;

        float directionY = Mathf.Sign(movementY);
        float rayLength = Mathf.Abs(movementY);

        int rayCount = 3;
        float raySpacing = col.bounds.size.x / (rayCount - 1);

        Vector2 rayOrigin;
        RaycastHit2D hit;
        for (int i = 0; i < rayCount; i++)
        {
            rayOrigin = (Vector2)transform.position + new Vector2(-col.bounds.extents.x, directionY * col.bounds.extents.y);
            rayOrigin += Vector2.right * raySpacing * i;

            hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
                return true;
        }

        return false;
    }

    public bool CheckHorizontal(float movementX)
    {
        if (movementX == 0)
            //rayLength = 0,1; directionX = -1;
            return true;

        float directionX = Mathf.Sign(movementX);
        float rayLength = Mathf.Abs(movementX);

        int rayCount = 3;
        float raySpacing = (col.bounds.size.y - 2 * verticalOffset) / (rayCount - 1);

        Vector2 rayOrigin;
        RaycastHit2D hit;
        for (int i = 0; i < rayCount; i++)
        {
            rayOrigin = (Vector2)transform.position + new Vector2(directionX * col.bounds.extents.x, -col.bounds.extents.y + verticalOffset);
            rayOrigin += Vector2.up * raySpacing * i;

            hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
                return true;
        }

        return false;
    }
}
