using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 6;
    public float damage;
    public float knockbackForce;
    public float lifeTime = 3;

	void Start () {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Vector2 direction = (transform.position - other.transform.position).normalized;

            var enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.hp != null)
            {
                enemy.hp.TryGetDamage(damage);
                enemy.Knockback(direction * knockbackForce);
            }
        }

        if (other.tag != "Player")
            Destroy(gameObject);
    }
}
