using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour {
    public Movement combatMovement;

    public void Move(Vector2 targetPos, Vector2 pos)
    {
        combatMovement.Move(targetPos, pos);
    }
}
