using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BubbleManager : MonoBehaviour
{
    public LoudnessDetector detector;
    public GameObject BubbleContainer;
    public GameObject bubblePrefab;
    public Transform spawnPosition;
    public int bubbleCount;
    public LayerMask bubbleMask;
    public int bubbleWinCount = 70;
    Collider2D[] colliders;
    public GameManagement GameManagement;

    int spawnAreaHeight = 30;
    int spawnAreaWidth = 20;

    int bubbleDetectionWidth = 20;
    int bubbleDetectionHeight = 10;
    float difficulty;
    public float difficultyScaling = 0.05f;
    float loudness;

    public AudioSource bubbleAudioSource;
    public float bubbleVolume = 0;
    float volumeDecreasingTimerInSeconds = 0f;
    float volumeDecreasingThreshholdInSeconds = 0.4f;
    private void Start()
    {
        SpawnBubbles(bubbleCount);
        difficulty = GlobalGameManager.Instance.difficulty;
    }

    void Update()
    {
        loudness = detector.GetLoudnessFromMicrophone();
        if (detector.IsBlowing(loudness))
        {
            bubbleVolume = Mathf.Clamp(bubbleVolume = Time.deltaTime, 0, 1);
            volumeDecreasingTimerInSeconds = 0;
            MoveBubbles();
        }
        else
        {
            //decrease the volume by 100% over 1 second after threshhold time has passed
            volumeDecreasingTimerInSeconds += Time.deltaTime;
            if (volumeDecreasingTimerInSeconds > volumeDecreasingThreshholdInSeconds)
            {
                bubbleVolume = Mathf.Clamp(bubbleVolume -= Time.deltaTime, 0, 1);
            }
        }
        bubbleAudioSource.volume = bubbleVolume;
        if (CountBubbles() >= bubbleWinCount)
        {
            detector.StopMicrophone();
            GlobalGameManager.Instance.WinGame();
        }
    }

    void MoveBubbles()
    {
        foreach (Transform child in BubbleContainer.transform)
        {
            child.GetComponent<BubbleMovement>().Move(loudness - Mathf.Clamp(loudness * difficulty * difficultyScaling, loudness / 3, loudness));
        }
    }

    void SpawnBubbles(int bubbleCount)
    {
        for (int i = 0; i < bubbleCount; i++)
        {
            GameObject NewBubble = Instantiate(bubblePrefab, new Vector3((float)spawnPosition.position.x + Random.RandomRange(-10f, 10f), (float)spawnPosition.position.y + Random.RandomRange(-50f, 5f), 0), Quaternion.identity);
            //sets a random starting momentum
            NewBubble.GetComponent<BubbleMovement>().sidewaysForce = Random.Range(-500, 501);
            //sets a random size
            NewBubble.transform.localScale = NewBubble.transform.localScale * Random.Range(1.0f, 1.6f);
            //sets layer to Bubble
            NewBubble.layer = 8;
            //makes bubble a child of BubbleContainer
            NewBubble.transform.parent = BubbleContainer.transform;
        }
    }

    int CountBubbles()
    {
        colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - bubbleDetectionWidth / 2, transform.position.y - bubbleDetectionHeight / 2), new Vector2(transform.position.x + bubbleDetectionWidth / 2, transform.position.y + bubbleDetectionHeight / 2), bubbleMask);
        return colliders.Length;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(new Vector2(spawnPosition.position.x, spawnPosition.position.y - spawnAreaHeight / 2), new Vector2(spawnAreaWidth, spawnAreaHeight));
        Gizmos.DrawWireCube(transform.position, new Vector2(bubbleDetectionWidth, bubbleDetectionHeight));
    }
}
