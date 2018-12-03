using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFlyToTarget : Movement {
    public float turnSpeed = 5;
    public float sensorRange = 3;
    protected float velocityYSmoothing;

    public override float GetDirectionX()
    {
        return Mathf.Sign(rigid.velocity.x);
    }

    public override void Move(Vector2 target, Vector2 position)
    {
        Vector2 velocity = rigid.velocity;
        Vector2 targetDirection = (target - position).normalized;

        Vector2 targetVelocity = VelocityToAwoidObstacles(velocity, targetDirection);

        //adjust speed
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocityXSmoothing, accelerationTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocity.y, ref velocityYSmoothing, accelerationTime);

        if (NeedToTurn(Mathf.Sign(velocity.x)))  //FIXME do i need this
        {
            velocity.x = -velocity.x;
        }

        rigid.velocity = velocity;
    }

    protected Vector2 VelocityToAwoidObstacles(Vector2 currentVelocity, Vector2 direction)
    {
        Vector2 targetVelocity = moveSpeed * direction;

        float directionX = Mathf.Sign(currentVelocity.x);
        float directionY = Mathf.Sign(currentVelocity.y);

        Vector2 horizontalRayDir = (directionX < 0) ? Vector2.left : Vector2.right;
        Vector2 verticalRayDir = (directionY < 0) ? Vector2.down : Vector2.up;

        RaycastHit2D horizontalHit = Physics2D.Raycast(transform.position, horizontalRayDir, sensorRange, checker.collisionMask);
        RaycastHit2D verticalHit = Physics2D.Raycast(transform.position, verticalRayDir, sensorRange, checker.collisionMask);

        Debug.DrawRay(transform.position, horizontalRayDir * sensorRange, Color.red);
        Debug.DrawRay(transform.position, verticalRayDir * sensorRange, Color.red);

        //the idea is from potential fields - move away from obstacles in normal's direction
        if (horizontalHit)
        {
            targetVelocity += horizontalHit.normal * turnSpeed;
        }

        if (verticalHit)
        {
            targetVelocity += verticalHit.normal * turnSpeed;
        }

        return targetVelocity;
    }

    protected bool NeedToTurn(float directionX)
    {
        return checker.CheckHorizontal(directionX * moveSpeed * Time.deltaTime);
    }
}
