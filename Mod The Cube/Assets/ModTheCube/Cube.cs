using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;
    public Vector3 position = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public Color color;
    public float rotateSpeed = 10f;
    public Vector3 rotateAngle = Vector3.forward;

    public Vector3 positionMin;
    public Vector3 positionMax;

    public Vector3 scaleMin;
    public Vector3 scaleMax;

    public float rotateSpeedMin;
    public float rotateSpeedMax;

    public Vector3 rotateAngleMin;
    public Vector3 rotateAngleMax;

    void Start()
    {
        transform.position = RandomVector3(positionMin, positionMax);
        transform.localScale = RandomVector3(scaleMin, scaleMax);
        Material material = Renderer.material;
        material.color = Random.ColorHSV();
    }

    void Update()
    {
        float rotateSpeedRandom = Random.Range(rotateSpeedMin, rotateSpeedMax);
        Vector3 rotateAngleRandom = RandomVector3(rotateAngleMin, rotateAngleMax);
        transform.Rotate(rotateAngleRandom * rotateSpeedRandom * Time.deltaTime);

        // Bellow code change properties in realtime in editor
        // transform.Rotate(rotateAngle * rotateSpeed * Time.deltaTime);
        // UpdateProperties();
    }

    public void UpdateProperties()
    {
        transform.position = position;
        transform.localScale = scale;
        Material material = Renderer.material;
        material.color = color;
    }

    public Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        float randomX = Random.Range(min.x, max.x);
        float randomY = Random.Range(min.y, max.y);
        float randomZ = Random.Range(min.z, max.z);
        return new Vector3(randomX, randomY, randomZ);
    }
}
