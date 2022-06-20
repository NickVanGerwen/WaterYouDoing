using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundPlay : MonoBehaviour
{
    public AudioMixerSnapshot background;

    private float m_Transition;

    // Use this for initialization
    void Start()
    {
        m_Transition = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        background.TransitionTo(m_Transition);
    }
}
