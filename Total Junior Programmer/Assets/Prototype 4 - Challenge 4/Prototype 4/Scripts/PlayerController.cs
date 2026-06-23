using System.Collections;
using JpPrototype4;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public GameObject powerUpIndicator;
    private InputSystem_Actions controls;
    [SerializeField] private Rigidbody playerRb;
    private GameObject focalPoint;
    // private float powerUpStrength = 1000;
    // public bool hasPowerup = false;
    // private IEquippable equippedAbility;
    private Coroutine abilityTimeoutCoroutine;
    private float nextReadyTime;
    private void Awake()
    {
        controls = new InputSystem_Actions();
        focalPoint = GameObject.Find("Focal Point");
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y;
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * playerSpeed * Time.deltaTime);
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        // if (equippedAbility != null && controls.Player.UseAbility.triggered)
        // {
        //     if (Time.time >= nextReadyTime)
        //     {
        //         // equippedAbility.OnUse(this);
        //     }
        // }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Powerup"))
    //     {
    //         powerUpIndicator.gameObject.SetActive(true);
    //         hasPowerup = true;
    //         Destroy(other.gameObject);
    //         StartCoroutine(LoseAbilityAfterTime(equippedAbility.Duration));
    //     }
    // }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
    //     {
    //         Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
    //         Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
    //         enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Force);
    //         Debug.Log("ABC");
    //     }
    // }

    public IEnumerator LoseAbilityAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        // ResetAbility();
    }

    // public void ResetAbility()
    // {
    //     if (equippedAbility != null)
    //     {
    //         equippedAbility.OnUnequip();
    //         equippedAbility = null;
    //     }
    //     powerUpIndicator.gameObject.SetActive(false);
    //     StopCoroutine(abilityTimeoutCoroutine);
    // }

    // public void EquipAbility(IEquippable ability)
    // {
    //     ResetAbility();
    //     equippedAbility = ability;
    //     abilityTimeoutCoroutine = StartCoroutine(LoseAbilityAfterTime(equippedAbility.Duration));
    // }
}
