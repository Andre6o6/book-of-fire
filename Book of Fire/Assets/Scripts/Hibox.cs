using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hibox : MonoBehaviour {
    public float damage;
    public float knockbackForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            var hp = other.GetComponent<Health>();
            if (hp != null)
            {
                hp.TryGetDamage(damage);
            }
        }

        if (other.tag == "Enemy")
        {
            Vector2 direction = (transform.position - other.transform.position).normalized;

            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Knockback(direction * knockbackForce);
            }
        }
    }
}
