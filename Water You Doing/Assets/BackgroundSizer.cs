using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackgroundSizer : MonoBehaviour
{
    public GameObject Background;
    public GameObject FactContainer;

    private TouchController controls;

    private void Awake()
    {
        controls = new TouchController();
        controls.Touch.SkipFact.performed += ctx => SkipFact();
    }
    void Start()
    {
        float backgroundScaler = GetComponent<RectTransform>().rect.width / Background.GetComponent<RectTransform>().rect.width;

        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width, Background.GetComponent<RectTransform>().rect.height * backgroundScaler);
        FactContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().rect.width * 0.7f, FactContainer.GetComponent<RectTransform>().rect.height);
        FactContainer.GetComponent<TextMeshProUGUI>().fontSize = (int)(GetComponent<RectTransform>().rect.width / 25);
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void SkipFact()
    {
        Debug.Log("skipped");
        gameObject.GetComponentInParent<GlobalGameManager>().BackToMainScreen();
    }

    void Update()
    {

    }
}
