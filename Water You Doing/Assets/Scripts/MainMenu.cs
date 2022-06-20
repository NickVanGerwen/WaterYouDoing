using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //public GameObject GlitchEffect;

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
   
    public void PlayGame()
    {
        GlobalGameManager.Instance.StartGame();
    }

    public void Tips()
    {
        SceneManager.LoadScene("tips");
    }

    private void Start()
    {
        //float backgroundScaler = GetComponent<RectTransform>().rect.width / GlitchEffect.GetComponent<RectTransform>().rect.width;
        //GlitchEffect.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, GlitchEffect.GetComponent<RectTransform>().rect.height * backgroundScaler);
    }
}
