using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {

    bool canShoot = true;
    [HideInInspector] public bool attacking;

    public Projectile projectile;
    public Vector3 offset;
    public float preTime;
    public float recoverTime;

    public bool TryShoot()
    {
        if (!canShoot)
            return false;

        StartCoroutine(Shoot());
        return true;
    }

    private IEnumerator Shoot()
    {
        
        canShoot = false;

        attacking = true;
        yield return new WaitForSeconds(preTime);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        Instantiate(projectile, transform.position + offset, Quaternion.Euler(0,0, angle));

        attacking = false;

        yield return new WaitForSeconds(recoverTime);
        canShoot = true;
    }
}
