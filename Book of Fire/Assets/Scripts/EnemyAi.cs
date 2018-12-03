using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour {
    public Player player;
    public Transform baseTarget;

    public float sightRange;
    public LayerMask obstacleMask;

    bool hostile = false;
    public float calmDownTime;
    float lastAgroTime;
    public float calmDownRange;

    public void UpdateState()
    {
        if (hostile)
            lastAgroTime += Time.deltaTime;

        CheckHostile();

    }

    public bool isHostile()
    {
        return hostile;
    }

    private void CheckHostile()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //player is out of potential agro range
        if (distanceToPlayer > calmDownRange)
        {
            hostile = false;
            return;
        }

        //player is in range and visible
        if (distanceToPlayer < sightRange)
        {
            Vector2 rayDirection = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, distanceToPlayer, obstacleMask);
            if (!hit)
            {
                lastAgroTime = 0;
                hostile = true;
                return;
            }
        }

        //else check time since last agro
        if (hostile && lastAgroTime > calmDownTime)
        {
            hostile = false;
        }
    }

    public Transform GetTarget()
    {
        if (hostile)
            return player.transform;
        else
            return baseTarget;
    }
}
