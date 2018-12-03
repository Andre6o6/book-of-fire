using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {
    bool canHit = true;
    public float recoverTime;
    public float attackTime;

    [HideInInspector]    public bool attacking = false;

    public bool TryHit()
    {
        if (!canHit)
            return false;

        StartCoroutine(Recover());
        return true;
    }

    private IEnumerator Recover()
    {
        canHit = false;

        attacking = true;
        yield return new WaitForSeconds(attackTime);
        attacking = false;

        yield return new WaitForSeconds(recoverTime);
        canHit = true;
    }
}
