using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth;
    public float health;

    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Slider preHealth;

    [HideInInspector]   public float expectedDamage;

    bool canGetDamage = true;
    public float recoverTime;

    public float timeToHeal;
    float healTime = 0;

    public float healSphereRadius = 2;
    public float healPerCorpse = 10;

    public GameObject corpse;
    public Action OnDeath;

    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (healthBar != null)
            healthBar.value = Mathf.Max(health, 0) / maxHealth;
    }

    private void Update()
    {
        if (preHealth != null)
            preHealth.value = Mathf.Max(health - expectedDamage, 0) / maxHealth;

        float target = Mathf.Max(health, 0) / maxHealth;
        if (healthBar != null)
            healthBar.value = Mathf.SmoothStep(healthBar.value, target, 0.1f);
    }

    // HEALING
    public void HealCharge()
    {
        healTime += Time.deltaTime;

        if (healTime >= timeToHeal)
        {
            healTime = 0;
            Heal();
        }
    }

    public void HealReset()
    {
        healTime = 0;
    }

    public void Heal()
    {
        float hp = 0;

        var cols = Physics2D.OverlapCircleAll(transform.position, healSphereRadius);
        foreach (var col in cols)
        {
            if (col.tag == "Corpse")
            {
                hp += healPerCorpse;
                Destroy(col.gameObject);
            }
        }

        health += hp;
        if (health > maxHealth)
            health = maxHealth;
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
        if (preHealth != null)
            preHealth.value = 0;

        if (healthBar != null)
            healthBar.value = 0;

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
