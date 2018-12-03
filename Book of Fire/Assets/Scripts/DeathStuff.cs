using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStuff : MonoBehaviour {

	void Start () {
        StartCoroutine(ToDeathScreen());
    }

    IEnumerator ToDeathScreen()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScreen");
    }
}
