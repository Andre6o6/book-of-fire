using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject howText;
    public void ExitClick()
    {
        Application.Quit();
    }

    public void PlayClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void How()
    {
        howText.SetActive(true);
    }
    public void HowClose()
    {
        howText.SetActive(false);
    }
}