using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f; 
    public float acceleration = 0.1f; 
    public float jumpForce = 7f; 
    public float maxForwardSpeed = 20f; 
    public float maxHeight = 25f; 
    public float minHeight = -3f; 
    public GameObject bulletPrefab;
    public float shootDuration = 5f; 
    public float speedBoostDuration = 5f; 
    public float speedBoostMultiplier = 2f;
    [SerializeField]private ParticleSystem speedBoostParticles;
    private MeshCollider shipCollider;
    

    private Rigidbody rb;
    public bool isShooting = false;
    public bool isSpeedBoosted = false;
    private float originalSpeed;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = forwardSpeed;
        shipCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && transform.position.y < maxHeight)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }
    }

    void FixedUpdate()
    {
        forwardSpeed = Mathf.Min(forwardSpeed + acceleration * Time.fixedDeltaTime, maxForwardSpeed);
        rb.velocity = new Vector3(forwardSpeed, rb.velocity.y, rb.velocity.z);
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        transform.position = clampedPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Collected: " + other.gameObject.name);

            if (other.gameObject.name.Contains("ShootingPowerUp"))
            {
                StartCoroutine(ShootCoroutine());
            }
            else if (other.gameObject.name.Contains("SpeedBoostPowerUp"))
            {
                StartCoroutine(SpeedBoostCoroutine());
                speedBoostParticles.Play();
            }

            Destroy(other.gameObject);
        }
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        float elapsed = 0f;

        while (elapsed < shootDuration)
        {
            Shoot();
            elapsed += 0.5f; // Shoot every 0.5 seconds
            yield return new WaitForSeconds(0.5f);
        }

        isShooting = false;
    }

    void Shoot()
    {
        if (isShooting)
        {
            Vector3 spawnPosition = transform.position + new Vector3(1, 0, 0);
            Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator SpeedBoostCoroutine()
    {
        isSpeedBoosted = true;
        originalSpeed = forwardSpeed;
        shipCollider.isTrigger = true;
        forwardSpeed *= speedBoostMultiplier;
        float elapsed = 0f;

        while (elapsed < speedBoostDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        forwardSpeed = originalSpeed;
        isSpeedBoosted = false;
        shipCollider.isTrigger = false;
    }
    
}
