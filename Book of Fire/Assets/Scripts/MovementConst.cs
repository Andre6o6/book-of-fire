using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementConst : Movement
{
    public bool ignoreGap;

    private void Start()
    {
        directionX = Mathf.Sign(Random.Range(-1f, 1f));
    }

    public override float GetDirectionX()
    {
        return directionX;
    }

    public override void Move(Vector2 target, Vector2 position)
    {
        Vector2 velocity = rigid.velocity;
        bool grounded = checker.CheckVertical(velocity.y * Time.deltaTime);

        if (velocity.x != moveSpeed && grounded)
        {
            float targetVelocityX = directionX * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        }

        if (NeedToTurn(velocity, grounded))
        {
            directionX = -directionX;
            velocity.x = directionX * moveSpeed;
        }

        rigid.velocity = velocity;
    }

    private bool NeedToTurn(Vector2 velocity, bool grounded)
    {
        bool needToTurn = false;

        //check raycaster's collisions for block
        if (checker.CheckHorizontal(velocity.x * Time.deltaTime))
        {
            needToTurn = true;
        }

        if (!ignoreGap && grounded)
        {
            //check raycast from bottom for gap
            float rayLength = 2;    //FIXME refine this constant
            Vector2 rayOrigin = (Vector2)transform.position + new Vector2(directionX * checker.col.bounds.extents.x, -checker.col.bounds.extents.y);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, checker.collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.blue);

            if (!hit)
            {
                needToTurn = true;
            }
        }

        return needToTurn;
    }
}
