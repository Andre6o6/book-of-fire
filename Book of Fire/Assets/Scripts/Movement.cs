using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float moveSpeed = 4;
    public float accelerationTime = .5f;
    protected float velocityXSmoothing;

    [HideInInspector]   public float directionX = 0;
    [HideInInspector]   public Rigidbody2D rigid;

    protected CollisionChecker checker;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        checker = GetComponent<CollisionChecker>();
    }

    public virtual void Move(Vector2 target, Vector2 position)
    {
    }

    public virtual float GetDirectionX()
    {
        return 0;
    }
}
