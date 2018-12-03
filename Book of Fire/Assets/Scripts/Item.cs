using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public Player player;
    public float range;
    public int index;
    public UnityEngine.UI.Text text;

    void Update () {
        if (Vector2.Distance(transform.position, player.transform.position) < range)
        {
            text.enabled = true;
            if (Input.GetKey(KeyCode.E))
            {
                player.GetItem(index);
                Destroy(gameObject);
            }
        }
        else
        {
            text.enabled = false;
        }
    }
}
