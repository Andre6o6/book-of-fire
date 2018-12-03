using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.Hit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            player.Shoot();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            player.Heal();         
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            player.HealReset();
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.Move(input);
    }
}
