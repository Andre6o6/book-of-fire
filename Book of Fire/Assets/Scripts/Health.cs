using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float health;

    bool canGetDamage = true;
    public float recoverTime;

    public GameObject corpse;
    public Action OnDeath;

    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    //GETTING DAMAGE
    public bool TryGetDamage(float dmg)
    {
        if (canGetDamage)
        {
            GetDamage(dmg);
            return true;
        }

        return false;
    }

    public void GetDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            //health = 0;
            Die();
            return;
        }

        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        canGetDamage = false;

        var layer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("recovering");

        //TODO make other effects
        int n = (int)(recoverTime / 0.1f);
        for (int i = 0; i < n; i++)
        {
            sprite.color = (i % 2 == 0) ? Color.red : Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        sprite.color = Color.white;

        gameObject.layer = layer;
        canGetDamage = true;
    }

    private void Die()
    {
        if (this.gameObject.tag != "Player")
            Destroy(gameObject);
        else
            gameObject.SetActive(false);

        if (OnDeath != null)
        {
            OnDeath();
        }

        if (corpse != null /*&& health > -10*/)
        {
            var obj = (GameObject)Instantiate(corpse, transform.position, transform.rotation);
            var rigid = obj.GetComponent<Rigidbody2D>();
            rigid.velocity = GetComponent<Rigidbody2D>().velocity;
        }
    }
}
