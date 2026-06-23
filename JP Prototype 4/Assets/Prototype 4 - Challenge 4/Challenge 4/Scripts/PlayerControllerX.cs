using System.Collections;
using System.Collections.Generic;
using JpPrototype4;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Challenge4
{
    public class PlayerControllerX : MonoBehaviour
    {
        public ParticleSystem boostParticle;
        private Rigidbody playerRb;
        private float speed = 500;
        private GameObject focalPoint;
        public float boostStrength = 20;
        public bool hasPowerup;
        public GameObject powerupIndicator;
        public int powerUpDuration = 5;
        public bool canBoost = true;
        private float normalStrength = 10; // how hard to hit enemy without powerup
        private float powerupStrength = 25; // how hard to hit enemy with powerup
        public InputAction boostInputAction;
        private InputSystem_Actions controls;

        void Awake()
        {
            controls = new InputSystem_Actions();
        }

        void OnEnable()
        {
            controls.Player.Enable();
            boostInputAction.Enable();
        }

        void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            focalPoint = GameObject.Find("Focal Point");
        }

        void Update()
        {
            // Add force to player in direction of the focal point (and camera)
            float verticalInput = controls.Player.Move.ReadValue<Vector2>().y;
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);

            // Set powerup indicator position to beneath player
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
            boostParticle.transform.position = transform.position;
            if (boostInputAction.triggered && canBoost)
            {
                playerRb.AddForce(focalPoint.transform.forward * boostStrength, ForceMode.Impulse);
                boostParticle.Play();
                canBoost = false;
                StartCoroutine(BoostCooldown());
            }
        }

        // If Player collides with powerup, activate powerup
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Powerup"))
            {
                Destroy(other.gameObject);
                hasPowerup = true;
                powerupIndicator.SetActive(true);
                StartCoroutine(PowerupCooldown());
            }
        }

        // Coroutine to count down powerup duration
        IEnumerator PowerupCooldown()
        {
            yield return new WaitForSeconds(powerUpDuration);
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }

        IEnumerator BoostCooldown()
        {
            yield return new WaitForSeconds(5);
            canBoost = true;
            Debug.Log("Can Boost");
        }

        // If Player collides with enemy
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

                if (hasPowerup) // if have powerup hit enemy with powerup force
                {
                    enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
                }
                else // if no powerup, hit enemy with normal strength 
                {
                    enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
                }
            }
        }
    }
}