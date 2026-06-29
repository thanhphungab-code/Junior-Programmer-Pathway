using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prototype2
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float speed = 20.0f;
        [SerializeField] private float xRange = 15;
        [SerializeField] private GameObject projectilePrefab;
        private float horizontalInput;

        // Update is called once per frame
        void Update()
        {
            // Check for left and right bounds
            if (transform.position.x < -xRange)
            {
                transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
            }

            if (transform.position.x > xRange)
            {
                transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
            }

            // Player movement left to right
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                // No longer necessary to Instantiate prefabs
                // Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);

                // Get an object object from the pool
                GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
                if (pooledProjectile != null)
                {
                    pooledProjectile.SetActive(true); // activate it
                    pooledProjectile.transform.position = transform.position; // position it at player
                }
            }

        }
    }
}