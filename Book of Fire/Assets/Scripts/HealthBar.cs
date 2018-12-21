using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    Health hp;

    public UnityEngine.UI.Slider healthBar;
    public UnityEngine.UI.Slider preHealth;

    [HideInInspector] public float expectedDamage;

    private void Awake()
    {
        hp = GetComponent<Health>();
        hp.OnDeath += () => { healthBar.value = 0; preHealth.value = 0; };
    }

    private void Start()
    {
        if (healthBar != null)
            healthBar.value = Mathf.Max(hp.health, 0) / hp.maxHealth;
    }

    private void Update()
    {
        if (preHealth != null)
            preHealth.value = Mathf.Max(hp.health - expectedDamage, 0) / hp.maxHealth;

        float target = Mathf.Max(hp.health, 0) / hp.maxHealth;
        if (healthBar != null)
            healthBar.value = Mathf.SmoothStep(healthBar.value, target, 0.1f);
    }
}
