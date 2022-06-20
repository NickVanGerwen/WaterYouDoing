using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    [SerializeField] private CanvasGroup myUIalpha;
    public Button myButton;

    public void HideUI()
    {
        myUIalpha.alpha = 0;
    }

   private void Awake()
   {
       Time.timeScale = 0f;
   }

   private void OnEnable()
   {
        myButton.onClick.AddListener(StartGame);
   }

   private void OnDisable()
   {
      myButton.onClick.RemoveListener(StartGame);
   }

   private void StartGame()
   {
      Time.timeScale = 1f;
      myButton.gameObject.SetActive(false);
   }
}
