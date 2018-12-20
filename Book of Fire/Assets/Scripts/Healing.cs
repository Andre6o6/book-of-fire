using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour {
    Health hp;

    public float timeToHeal;
    float healTime = 0;

    public float healSphereRadius = 2;
    public float healPerCorpse = 10;

    private void Awake()
    {
        hp = GetComponent<Health>();
    }

    public void HealCharge()
    {
        healTime += Time.deltaTime;

        if (healTime >= timeToHeal)
        {
            healTime = 0;
            HealViaCorpses();
        }
    }

    public void HealReset()
    {
        healTime = 0;
    }

    public void HealViaCorpses()
    {
        float heal = 0;

        var cols = Physics2D.OverlapCircleAll(transform.position, healSphereRadius);
        foreach (var col in cols)
        {
            if (col.tag == "Corpse")
            {
                heal += healPerCorpse;
                Destroy(col.gameObject);
            }
        }

        Heal(heal);
    }

    public void Heal(float heal)
    {
        if (hp == null)
            return;

        hp.health += heal;
        if (hp.health > hp.maxHealth)
            hp.health = hp.maxHealth;
    }
}
