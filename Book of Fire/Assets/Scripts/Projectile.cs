using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject fire;

    public float speed = 6;
    public float damage;
    public float knockbackForce;
    public float lifeTime = 3;

	void Start () {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);

        //var obj = Instantiate(fire, transform.position, new Quaternion());
        //Destroy(obj, 1);
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
        else if (other.tag == "Breakable")
        {
            var hp = other.GetComponent<Health>();
            hp.TryGetDamage(damage);
        }

        if (other.tag != "Player")
            Destroy(gameObject);

        var obj = Instantiate(fire, transform.position, new Quaternion());
        Destroy(obj, 0.5f);
    }
}
