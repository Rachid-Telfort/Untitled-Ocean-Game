using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    const uint maxPointCount = 100;

    static bool isPlaying = false;
    bool gameOver;
    uint lifeCount;
    uint pointCount;

    [SerializeField]
    TextMeshProUGUI livesGUI;

    [SerializeField]
    TextMeshProUGUI pointsGUI;

    [SerializeField]
    AudioClip itemCollectClip;

    [SerializeField]
    AudioClip itemMissedClip;

    [SerializeField]
    AudioClip youLoseClip;

    [SerializeField]
    AudioSource backgroundSource;

    AudioSource canvasSource;
    Scene currentScene;

    void Awake()
    {
        DontDestroyOnLoad(backgroundSource);

        if(!isPlaying)
        {
            backgroundSource.Play();
            isPlaying = true;
        }
        
        canvasSource = GetComponent<AudioSource>();

        gameOver = false;
        lifeCount = 3;
        pointCount = 0;

        livesGUI.text = "Lives : 3";
        pointsGUI.text = "Trash : 0";

        currentScene = SceneManager.GetActiveScene();
    }

    public void UpdatePointCount()
    {
        if (pointCount < maxPointCount)
        {
            ++pointCount;
            pointsGUI.text = "Trash : " + pointCount;

            canvasSource.clip = itemCollectClip;
            canvasSource.Play();
        }
    }

    public void UpdateLifeCount()
    {
        canvasSource.Stop();

        if(lifeCount > 0)
        {
            --lifeCount;
            livesGUI.text = "Lives : " + lifeCount;

            canvasSource.clip = itemMissedClip;
            canvasSource.Play();
        }

        if(lifeCount == 0)
        {
            canvasSource.clip = youLoseClip;
            canvasSource.Play();

            SceneManager.LoadScene(currentScene.name);
        }
    }
}
