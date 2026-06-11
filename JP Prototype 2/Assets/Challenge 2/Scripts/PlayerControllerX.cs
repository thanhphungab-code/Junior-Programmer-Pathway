using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    public float spawnDelay;
    private float spawnCountDown;

    void Update()
    {
        spawnCountDown -= Time.deltaTime;
        if (spawnCountDown <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            spawnCountDown = spawnDelay;
        }
    }
}
