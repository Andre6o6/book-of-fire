using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Player player;
    public float price;
    public float range;

    Animator anim;
    SpriteRenderer sprite;
    bool active = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(sprite.color.a, 1, 0.1f));
            anim.SetTrigger("on");
            active = true;
            player.hpBar.expectedDamage = price;
        }
        else
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(sprite.color.a, 0, 0.2f));
            anim.SetTrigger("off");
            active = false;
            player.hpBar.expectedDamage = 0;
        }


        if (active && Input.GetKey(KeyCode.E))
        {
            anim.SetTrigger("open");
            player.hp.GetDamage(price);
            player.hpBar.expectedDamage = 0;
            Destroy(this);
        }
    }
}
