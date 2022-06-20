using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoudnessDetector : MonoBehaviour
{
    string audioinput;
    int sampleWindow = 64;
    AudioClip AudioClip;
    float threshold = 0.01f;

    private void Start()
    {
        MicrophoneToAudioClip();
    }

    //converts the microphone input to an audioclip
    public void MicrophoneToAudioClip()
    {
        audioinput = Microphone.devices[0].ToString();
        AudioClip = Microphone.Start(audioinput, true, 20, AudioSettings.outputSampleRate);
    }

    public void StopMicrophone()
    {
        Microphone.End(audioinput);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromClip(Microphone.GetPosition(Microphone.devices[0]), AudioClip);
    }

    //calculates the loudness of an audioclip
    float GetLoudnessFromClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0)
            return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        return totalLoudness / sampleWindow;
    }

    //decides if the input is loud enough to be considered blowing
    public bool IsBlowing(float loudness)
    {
        return loudness > threshold;
    }
}
