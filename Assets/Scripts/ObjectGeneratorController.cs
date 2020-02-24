using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectGeneratorController : MonoBehaviour
{
    const float icebergAppearYCoordinate = 0.0f;
    const float icebergAppearZCoordinate = 30.0f;
    const float icebergColliderXCenterCoordinate = 17.0f;
    const float icebergColliderYCenterCoordinate = 0.0f;
    const float icebergColliderZCenterCoordinate = -1.0f;
    const float icebergColliderXSizeCoordinate = 4.7f;
    const float icebergColliderYSizeCoordinate = 3.0f;
    const float icebergColliderZSizeCoordinate = 5.0f;
    const float icebergLeftBoundary = -19.0f;
    const float icebergRightBoundary = 0.0f;
    const float trashAppearYCoordinate = 0.0f;
    const float trashAppearZCoordinate = 30.0f;
    const float trashColliderXCenterCoordinate = 36.0f;
    const float trashColliderYCenterCoordinate = 1.0f;
    const float trashColliderZCenterCoordinate = 1.0f;
    const float trashColliderXSizeCoordinate = 11.0f;
    const float trashColliderYSizeCoordinate = 1.0f;
    const float trashColliderZSizeCoordinate = 16.0f;
    const float trashLeftBoundary = -19.0f;
    const float trashRightBoundary = 2.0f;
    const float maxIcebergAppearInterval = 3.0f;
    const float minIcebergAppearInterval = 2.0f;
    const float maxIcebergSpeed = 10.0f;
    const float minIcebergSpeed = 5.0f;
    const float maxTrashAppearInterval = 3.0f;
    const float minTrashAppearInterval = 2.0f;
    const float maxTrashSpeed = 10.0f;
    const float minTrashSpeed = 5.0f;
    const uint maxIcebergCount = 100u;
    const uint maxTrashCount = 100u;
    const float seagulSoundInterval = 20.0f;

    float icebergAppearXCoordinate;
    float icebergAppearInterval;
    float icebergAppearTime;
    float icebergSpeed;
    float trashAppearXCoordinate;
    float trashAppearInterval;
    float trashAppearTime;
    float trashSpeed;
    float seagulTime;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    GameObject icebergObject;

    [SerializeField]
    GameObject icebergObject2;

    [SerializeField]
    GameObject icebergObject3;

    [SerializeField]
    GameObject trashObject;

    [SerializeField]
    GameObject trashObject2;

    [SerializeField]
    GameObject trashObject3;

    [SerializeField]
    AudioClip seagulClip;

    [SerializeField]
    AudioClip seagulClip2;

    AudioSource seagulSource;
    Transform objectTransform;
    BoxCollider objectCollider;

    List<float> icebergSpeeds;
    List<float> trashSpeeds;
    List<GameObject> collectibles;
    List<GameObject> obstacles;

    void Awake()
    {
        seagulTime = seagulSoundInterval;
        seagulSource = GetComponent<AudioSource>();

        icebergAppearInterval = UnityEngine.Random.Range(minIcebergAppearInterval, maxIcebergAppearInterval);
        trashAppearInterval = UnityEngine.Random.Range(minTrashAppearInterval, maxTrashAppearInterval);

        icebergAppearTime = icebergAppearInterval;
        trashAppearTime = trashAppearInterval;

        icebergSpeeds = new List<float>();
        trashSpeeds = new List<float>();
        collectibles = new List<GameObject>();
        obstacles = new List<GameObject>();
    }

    void FixedUpdate()
    {
        UpdateTrash();
        UpdateIcebergs();
        PlaySeaguls();

        int trashCount = collectibles.Count;

        for(int trashIndex = 0; trashIndex < trashCount; ++ trashIndex)
        {
            objectTransform = collectibles[trashIndex].GetComponent<Transform>();
            objectTransform.Translate(Vector3.back * trashSpeeds[trashIndex] / 100);
        }

        int icebergCount = obstacles.Count;

        for (int icebergIndex = 0; icebergIndex < icebergCount; ++ icebergIndex)
        {
            objectTransform = obstacles[icebergIndex].GetComponent<Transform>();
            objectTransform.Translate(Vector3.back * icebergSpeeds[icebergIndex] / 100);
        }
    }

    void UpdateIcebergs()
    {
        icebergAppearTime -= Time.deltaTime;
        int lastIcebergIndex = obstacles.Count;

        if (icebergAppearTime <= 0)
        {
            icebergAppearInterval = UnityEngine.Random.Range(minIcebergAppearInterval, maxIcebergAppearInterval);
            icebergAppearTime = icebergAppearInterval;
            icebergSpeed = UnityEngine.Random.Range(minIcebergSpeed, maxIcebergSpeed);

            if (lastIcebergIndex == maxIcebergCount)
            {
                lastIcebergIndex = 0;
                icebergSpeeds.Clear();
                obstacles.Clear();
            }

            icebergSpeeds.Add(icebergSpeed);

            int icebergChoose = UnityEngine.Random.Range(1, 3);

            if(icebergChoose == 1)
            {
                obstacles.Add(Instantiate(icebergObject));
            }

            else if(icebergChoose == 2)
            {
                obstacles.Add(Instantiate(icebergObject2));
            }

            else
            {
                obstacles.Add(Instantiate(icebergObject3));
            }

            obstacles[lastIcebergIndex].SetActive(true);
            obstacles[lastIcebergIndex].AddComponent(typeof(BoxCollider));

            objectTransform = obstacles[lastIcebergIndex].GetComponent<Transform>();
            objectTransform.position = Vector3.forward;

            objectCollider = obstacles[lastIcebergIndex].GetComponent<BoxCollider>();
            objectCollider.center = new Vector3(icebergColliderXCenterCoordinate, icebergColliderYCenterCoordinate, icebergColliderZCenterCoordinate);
            objectCollider.size = new Vector3(icebergColliderXSizeCoordinate, icebergColliderYSizeCoordinate, icebergColliderZSizeCoordinate);

            icebergAppearXCoordinate = UnityEngine.Random.Range(icebergLeftBoundary, icebergRightBoundary);
            objectTransform.position = new Vector3(icebergAppearXCoordinate, icebergAppearYCoordinate, icebergAppearZCoordinate);
        }
    }

    void UpdateTrash()
    {
        trashAppearTime -= Time.deltaTime;
        int lastTrashIndex = collectibles.Count;

        if (trashAppearTime <= 0)
        {
            trashAppearInterval = UnityEngine.Random.Range(minTrashAppearInterval, maxTrashAppearInterval);
            trashAppearTime = trashAppearInterval;
            trashSpeed = UnityEngine.Random.Range(minTrashSpeed, maxTrashSpeed);

            if (lastTrashIndex == maxTrashCount)
            {
                lastTrashIndex = 0;
                trashSpeeds.Clear();
                collectibles.Clear();
            }

            trashSpeeds.Add(trashSpeed);

            int trashChoose = UnityEngine.Random.Range(1, 3);

            if (trashChoose == 1)
            {
                collectibles.Add(Instantiate(trashObject));
            }

            else if (trashChoose == 2)
            {
                collectibles.Add(Instantiate(trashObject2));
            }

            else
            {
                collectibles.Add(Instantiate(trashObject3));
            }

            collectibles[lastTrashIndex].SetActive(true);
            collectibles[lastTrashIndex].AddComponent(typeof(BoxCollider));

            objectTransform = collectibles[lastTrashIndex].GetComponent<Transform>();
            objectTransform.position = Vector3.left * -9.94f;

            objectCollider = collectibles[lastTrashIndex].GetComponent<BoxCollider>();
            objectCollider.center = new Vector3(trashColliderXCenterCoordinate, trashColliderYCenterCoordinate, trashColliderZCenterCoordinate);
            objectCollider.size = new Vector3(trashColliderXSizeCoordinate, trashColliderYSizeCoordinate, trashColliderZSizeCoordinate);

            trashAppearXCoordinate = UnityEngine.Random.Range(trashLeftBoundary, trashRightBoundary);
            objectTransform.position = new Vector3(trashAppearXCoordinate, trashAppearYCoordinate, trashAppearZCoordinate);
        }
    }

    void PlaySeaguls()
    {
        seagulTime -= Time.deltaTime;

        if(seagulTime <= 0)
        {
            seagulTime = seagulSoundInterval;
            seagulSource.Stop();

            int seagulChoose = UnityEngine.Random.Range(1, 2);

            if(seagulChoose == 1)
            {
                seagulSource.clip = seagulClip;
            }

            else
            {
                seagulSource.clip = seagulClip2;
            }

            seagulSource.Play();
        }
    }
}
