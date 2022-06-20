using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    public Transform winScreen;
    public Transform loseScreen;
    public string[] scenes;
    List<string> availableScenes;
    bool gameBeingPlayed = true;
    public bool TimerWin = false;

    float timer;
    public float maxTime = 10f;
    public Slider slider;

    private Coroutine endRoutine;

    private void Start()
    {
        timer = maxTime;
    }

    private void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (TimerWin)
            {
                WinGame();
            }
            else
            {
                LoseGame();
            }
        }
        else
        {
            slider.GetComponent<Slider>().value = 1 - (maxTime - timer) / maxTime;

            // Debug.Log((maxTime - timer) / maxTime);
        }
    }

    public void WinGame()
    {
        if (gameBeingPlayed)
        {
            gameBeingPlayed = false;

            winScreen.gameObject.SetActive(true);

            //count points idk whatever 

            if (endRoutine == null)
                endRoutine = StartCoroutine(WaitForNewGame());
        }
    }

    public void LoseGame()
    {
        if (gameBeingPlayed)
        {
            gameBeingPlayed = false;

            loseScreen.gameObject.SetActive(true);

            Time.timeScale = 0.1f;
            //take lives take ass idk whatever 

            if (endRoutine == null)
                endRoutine = StartCoroutine(WaitForNewGame());
        }
    }

    IEnumerator WaitForNewGame()
    {
        yield return new WaitForSeconds(3 * Time.timeScale);
        //StopCoroutine(WaitForNewGame());
        //Restart();
        //LoadNewGame();
        Time.timeScale = 1f;

        Test();
    }

    private void Test()
    {
        StopCoroutine(endRoutine);
        endRoutine = null;
        LoadNewGame();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNewGame()
    {
        availableScenes = new List<string>();
        foreach (string scene in scenes)
        {
            if (scene != SceneManager.GetActiveScene().name)
            {
                availableScenes.Add(scene);
            }
        }
        SceneManager.LoadScene(availableScenes[Random.Range(0, availableScenes.Count)]);
    }
}
