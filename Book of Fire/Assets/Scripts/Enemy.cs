using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [HideInInspector]   public Health hp;
    public Movement movement;

    EnemyAi ai;
    EnemyCombat combat;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
        ai = GetComponent<EnemyAi>();
        combat = GetComponent<EnemyCombat>();
    }

    private void Update()
    {
        bool hostile = false;
        Transform target = transform;
        float dir = 0;

        if (ai != null)
        {
            ai.UpdateState();
            hostile = ai.isHostile();
            target = ai.GetTarget();
        }

        if (hostile && combat != null)
        {
            combat.Move(target.position, transform.position);
            dir = combat.combatMovement.GetDirectionX();
        }
        else
        {
            if (movement != null)
            {
                movement.Move(target.position, transform.position);
                dir = movement.GetDirectionX();
            }
        }

        HandleMirroring(dir);
    }

    public void Knockback(Vector2 force)
    {
        rigid.velocity -= force;
    }

    private void HandleMirroring(float dir)
    {
        if (dir == 0)
            return;
        transform.localScale = new Vector3(dir, 1, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("contact damage"))
        {
            hp.GetDamage(100);
        }
    }
}
