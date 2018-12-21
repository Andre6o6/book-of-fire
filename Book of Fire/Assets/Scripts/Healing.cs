using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour {
    Health hp;
    public GameObject fire;
    ParticleSystem sphereParticle;

    public float timeToHeal;
    float healTime = 0;

    public float healSphereRadius = 2;
    public float healPerCorpse = 10;

    private void Awake()
    {
        hp = GetComponent<Health>();
        sphereParticle = GetComponent<ParticleSystem>();
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

                //Burn corpse
                col.tag = "Burning Corpse";
                var obj = Instantiate(fire, col.transform.position, new Quaternion());
                Destroy(obj, 2.5f);
                Destroy(col.gameObject, 2);
            }
        }

        sphereParticle.Play();
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
