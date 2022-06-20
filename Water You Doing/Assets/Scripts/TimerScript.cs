using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public bool TimerWin = false;
    public float maxTime = 10f;
    public float minTime = 5f;
    public Slider slider;

    public float difficultyScaling = 1f;
    public float difficulty;
    public float timer;
    bool gameOver = false;

    public GameObject Background;

    void Start()
    {
        float backgroundScaler = GetComponent<RectTransform>().rect.width / Background.GetComponent<RectTransform>().rect.width;

        slider.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, slider.GetComponent<RectTransform>().rect.height);
        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, Background.GetComponent<RectTransform>().rect.height * backgroundScaler);

        difficulty = GlobalGameManager.Instance.difficulty;

        //lengthens game time if goal is to survive the timer
        if (TimerWin)
            difficultyScaling = -difficultyScaling;

        maxTime -= (difficulty * difficultyScaling);
        if (maxTime < minTime)
            maxTime = minTime;

        timer = maxTime;
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !gameOver)
        {
            if (TimerWin)
            {
                GlobalGameManager.Instance.WinGame();
            }
            else
            {
                GlobalGameManager.Instance.LoseGame();
            }
            gameOver = true;
        }
        else
        {
            slider.GetComponent<Slider>().value = 1 - (maxTime - timer) / maxTime;
        }
    }

    public float GetCurrentTime()
    {
        return timer;
    }
}
