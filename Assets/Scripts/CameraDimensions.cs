using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDimensions : MonoBehaviour
{
    public static CameraDimensions Instance { get; private set; }

    private Camera _mainCamera;
    public float Width { get; private set; }
    public float Height { get; private set; }
    public Vector3 TopLeft { get; private set; }
    public Vector3 TopRight { get; private set; }
    public Vector3 BottomLeft { get; private set; }
    public Vector3 BottomRight { get; private set; }

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetDimensions();
    }

    private void SetDimensions()
    {
        float camHeight = 2f * _mainCamera.orthographicSize;
        float camWidth = camHeight * _mainCamera.aspect;

        Height = camHeight;
        Width = camWidth;

        Vector3 cameraCenter = _mainCamera.transform.position;
        TopLeft = cameraCenter + new Vector3(-camWidth / 2, camHeight / 2, 0);
        TopRight = cameraCenter + new Vector3(camWidth / 2, camHeight / 2, 0);
        BottomLeft = cameraCenter + new Vector3(-camWidth / 2, -camHeight / 2, 0);
        BottomRight = cameraCenter + new Vector3(camWidth / 2, -camHeight / 2, 0);
    }
    
    public Vector3 GetRandomTopPosition()
    {
        // Generate a random value between 0 and 1
        float randomValue = UnityEngine.Random.Range(0f, 1f);

        // Calculate the offset position for TopLeft and TopRight
        Vector3 offsetTopLeft = TopLeft + new Vector3(1, 0, 0); // 1 unit offset to the right from TopLeft
        Vector3 offsetTopRight = TopRight - new Vector3(1, 0, 0); // 1 unit offset to the left from TopRight

        // Interpolate between offsetTopLeft and offsetTopRight
        return Vector3.Lerp(offsetTopLeft, offsetTopRight, randomValue);
    }

}