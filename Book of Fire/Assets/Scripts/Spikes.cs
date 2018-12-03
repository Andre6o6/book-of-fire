using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    public bool forceIn;
    public bool forceOut;

    public float timeIn = 1;
    public float timeOut = 2;
    public float timeOffset;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Cicle());
    }

    IEnumerator Cicle()
    {
        yield return new WaitForSeconds(timeOffset);
        while (!forceIn)
        {
            anim.SetTrigger("out");
            yield return new WaitForSeconds(timeOut);
            if (forceOut)
                break;

            anim.SetTrigger("in");
            yield return new WaitForSeconds(timeIn);
        }
    }
}
