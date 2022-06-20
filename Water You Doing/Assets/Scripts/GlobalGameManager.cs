using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance;

    public Transform winScreen;
    public Transform loseScreen;
    public string[] scenes;
    public int StartingLives = 5;
    public float difficulty;
    float timer;
    public float timerLimit = 3.0f;
    private bool facttimer = false;

    List<string> availableScenes;
    bool gameBeingPlayed = true;
    private Coroutine endRoutine;
    public int remaingLives;

    public GameObject remlivesTXT;

    public bool alive;
    public Canvas factScreen;
    public GameObject FactText;
    public ScoreScript ScoreScriptObject;
    public Canvas ScoreCanvas;

    bool livesActive = false;

    private void Awake()
    {
        Microphone.Start(Microphone.devices[0], false, 1, 1);
        Microphone.End(Microphone.devices[0]);

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else Destroy(gameObject); //any time the instance is not null destory the gameobject that was just created.
    }

    private void Update()
    {
        if (!alive && livesActive)
        {
            if (facttimer)
            {
                timer += Time.deltaTime;
                Debug.Log(timer);
                if (timer >= timerLimit)
                {
                    facttimer = false;
                    Debug.Log("Loaded Menu");
                    timer = 0;
                    factScreen.gameObject.SetActive(false);
                    BackToMainScreen();
                }
            }

            else
            {
                facttimer = true;
                timer = 0;
            }

        }
    }

    public void StartGame()
    {
        //difficulty = 0;
        remaingLives = StartingLives;
        LoadNewGame();
        alive = true;
        timer = timerLimit;
        ScoreCanvas.gameObject.SetActive(true);
    }


    public void WinGame()
    {
        if (gameBeingPlayed)
        {
            difficulty++;
            gameBeingPlayed = false;

            winScreen.gameObject.SetActive(true);

            GameObject timercanvas = GameObject.Find("TimerCanvas");

            if (ScoreScriptObject != null && timercanvas != null)
            {
                TimerScript timer = timercanvas.GetComponent<TimerScript>();
                float maxminigametime;

                if (timer.TimerWin)
                {
                    maxminigametime = timer.maxTime;
                    ScoreScriptObject.AddPoint(maxminigametime, maxminigametime);
                }

                else
                {
                    maxminigametime = timer.maxTime;
                    ScoreScriptObject.AddPoint(timer.GetCurrentTime(), maxminigametime);
                }
            }
            if (endRoutine == null)
                endRoutine = StartCoroutine(WaitForNewGame());
        }
    }

    public void LoseGame()
    {

        if (gameBeingPlayed)
        {
            gameBeingPlayed = false;
            remaingLives--;
            Debug.Log("remaining lives: " + remaingLives);
            if (remaingLives <= 0)
            {
                alive = false;
            }
            if (alive)
            {
                difficulty++;

                gameBeingPlayed = false;

                loseScreen.gameObject.SetActive(true);

                Time.timeScale = 0.1f;

                if (endRoutine == null)
                    endRoutine = StartCoroutine(WaitForNewGame());
            }
            else
            {
                Time.timeScale = 0f;

                remlivesTXT = GameObject.FindGameObjectWithTag("LivesText");
                if (remlivesTXT != null)
                {
                    remlivesTXT.SetActive(false);
                }
                factScreen.gameObject.SetActive(true);
                FactText.GetComponent<TextMeshProUGUI>().text = GetComponent<FactRandomizer>().GetFact();
            }
        }

    }

    public void BackToMainScreen()
    {
        gameBeingPlayed = false;
        SceneManager.LoadScene("menu");
        factScreen.gameObject.SetActive(false);
        ScoreCanvas.gameObject.SetActive(false);

    }

    IEnumerator WaitForNewGame()
    {
        if (alive)
        {
            yield return new WaitForSeconds(3 * Time.timeScale);
            //StopCoroutine(WaitForNewGame());
            //Restart();
            //LoadNewGame();
            Time.timeScale = 1f;

            Test();
        }

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
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
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
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
        SceneManager.LoadScene(availableScenes[UnityEngine.Random.Range(0, availableScenes.Count)]);
        gameBeingPlayed = true;
    }
}
