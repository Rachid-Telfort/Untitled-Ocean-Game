using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float leftBoundary = -2.0f;
    const float rightBoundary = 11.0f;
    const float colliderStartXCoordinate = -4.30f;
    const float colliderStartYCoordinate = 0.0f;
    const float colliderStartZCoordinate = 0.40f;
    const float colliderSizeXCoordinate = 3.0f;
    const float colliderSizeYCoordinate = 2.0f;
    const float colliderSizeZCoordinate = 9.0f;
    const float startXCoordinate = 4.30f;
    const float startYCoordinate = 0.0f;
    const float startZCoordinate = 2.0f;

    bool applicationQuit;
    bool boatMoveLeft;
    bool boatMoveRight;
    bool hornClipPlay;

    [SerializeField]
    CanvasController canvasController;

    [SerializeField]
    GameObject playerObject;

    AudioSource playerSource;
    BoxCollider playerCollider;
    Transform playerTransform;

    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<BoxCollider>();
        playerTransform = GetComponent<Transform>();

        playerCollider.center = new Vector3(colliderStartXCoordinate, colliderStartYCoordinate, colliderStartZCoordinate);
        playerCollider.size = new Vector3(colliderSizeXCoordinate, colliderSizeYCoordinate, colliderSizeZCoordinate);
        playerTransform.position = new Vector3(startXCoordinate, startYCoordinate, startZCoordinate);

        playerObject.transform.position = playerTransform.position;
        playerObject.SetActive(true);
        playerObject.transform.SetParent(playerTransform);
    }

    void FixedUpdate()
    {
        boatMoveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);

        if(boatMoveLeft)
        {
            boatMoveLeft = playerTransform.position.x + Vector3.left.x > leftBoundary;

            if (boatMoveLeft)
            {
                playerTransform.Translate(Vector3.left);
            }
        }

        boatMoveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        if(boatMoveRight)
        {
            boatMoveRight = playerTransform.position.x + Vector3.right.x < rightBoundary;

            if (boatMoveRight)
            {
                playerTransform.Translate(Vector3.right);
            }
        }

        hornClipPlay = Input.GetKey(KeyCode.Space);

        if(hornClipPlay && !playerSource.isPlaying)
        {
            playerSource.Play();
        }

        applicationQuit = Input.GetKey(KeyCode.Escape);

        if(applicationQuit)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Trash")
        {
            collider.gameObject.SetActive(false);
            canvasController.UpdatePointCount();
        }

        if(collider.gameObject.tag == "Iceberg")
        {
            canvasController.UpdateLifeCount();
        }
    }
}
