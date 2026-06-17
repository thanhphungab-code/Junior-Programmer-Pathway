using System;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Prefab")]
    public GameObject ballPrefab;

    [Header("Fixed Spawn Position")]
    public float fixedX = 0f;
    public float fixedY = 5f;
    public float rangeZ = 4;
    [Header("Distance From Camera")]
    public float spawnDepth = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBall();
        }
    }

    void SpawnBall()
    {
        Vector3 mousePos = Input.mousePosition;

        // Khoảng cách từ camera tới mặt phẳng spawn
        mousePos.z = spawnDepth;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 spawnPos = new Vector3(
            fixedX,
            fixedY,
            worldPos.z
        );

        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }
}
