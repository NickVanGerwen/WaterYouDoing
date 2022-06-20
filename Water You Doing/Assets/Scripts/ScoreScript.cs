using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text scoreText;
    int score = 0;
    public int TimeLeft;
    void Start()
    {
        scoreText.text = "SCORE: " + score.ToString();
    }

    void Update() //I'm using update to test increasing score via a button press. This changes at a later moment.
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Debug.Log("P is pressed");
        //    AddPoint(TimeLeft);
        //}
    }

    public void AddPoint(float timeleft, float maxtime) //This function is the increase of the score
    {
        float gainedscore = (timeleft / maxtime) * 100;
        int gainedscoreint = (int)Mathf.Round(gainedscore);
        score += gainedscoreint;
        scoreText.text = "SCORE: " + score.ToString();
        Debug.Log(score);
    }
}
