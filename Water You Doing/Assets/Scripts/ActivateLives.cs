using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateLives : MonoBehaviour
{
    public GameObject remlivesTXT;
    public string[] scenes;
    bool livesActive;

    void Start()
    {
        remlivesTXT = GameObject.FindGameObjectWithTag("LivesText");
        Debug.Log(remlivesTXT.gameObject.name);
        Debug.Log(SceneManager.GetActiveScene().name);
        livesActive = Array.Exists(scenes, element => element == SceneManager.GetActiveScene().name);
        remlivesTXT.gameObject.SetActive(livesActive);

        if (livesActive)
            remlivesTXT.GetComponent<TextMeshProUGUI>().text = Convert.ToString("Lives:" + GlobalGameManager.Instance.remaingLives);
    }

    public void ActivateLivesText()
    {
        remlivesTXT.gameObject.SetActive(true);
    }
}
