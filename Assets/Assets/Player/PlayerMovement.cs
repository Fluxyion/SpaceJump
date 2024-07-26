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
    public float shootDuration = 8f; 
    public float speedBoostDuration = 15f; 
    public int speedBoostMultiplier = 2;
    [SerializeField]private ParticleSystem speedBoostParticles;
    private MeshCollider shipCollider;
    private int speedBoostMultiplierCount=0;
    

    private Rigidbody rb;
    public bool isShooting = false;
    public bool isSpeedBoosted = false;
    private float originalSpeed;
    public int activeSpeedBoosts = 0;
    private ScoreManager scoreManager;
    private int currentMultiplier;
    private BuffDurationUI _buffDurationUI;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = forwardSpeed;
        shipCollider = GetComponent<MeshCollider>();
        scoreManager = FindObjectOfType<ScoreManager>();
        _buffDurationUI = FindObjectOfType<BuffDurationUI>();
        
        currentMultiplier = 1;
        _buffDurationUI.SetSpeedBoostDuration(speedBoostDuration);
        _buffDurationUI.SetShootingBuffDuration(shootDuration);
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
        rb.velocity = new Vector3(forwardSpeed, rb.velocity.y, 0);
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        transform.position = clampedPosition;
        if (!isSpeedBoosted)
        {
            originalSpeed = forwardSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Collected: " + other.gameObject.name);

            if (other.gameObject.name.Contains("ShootingPowerUp"))
            {
                StartCoroutine(ShootCoroutine());
                _buffDurationUI.ShootingCoundown(shootDuration);
            }
            else if (other.gameObject.name.Contains("SpeedBoostPowerUp"))
            {
                StartCoroutine(SpeedBoostCoroutine());
                speedBoostParticles.Play();
                _buffDurationUI.SpeedBoostCoundown(speedBoostDuration);
            }
            else if (other.gameObject.name.Contains("MultiplierBoost"))
            {
                currentMultiplier++;
                if (!isSpeedBoosted)
                {
                    scoreManager.SetSpeedBoostMultiplier(currentMultiplier);
                }
                else
                {
                    scoreManager.SetSpeedBoostMultiplier(currentMultiplier*speedBoostMultiplierCount*speedBoostMultiplier);
                }
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
        activeSpeedBoosts++;
        speedBoostMultiplierCount++;
        isSpeedBoosted = true;
        shipCollider.isTrigger = true;
        forwardSpeed *= speedBoostMultiplier;
        scoreManager.SetSpeedBoostMultiplier(speedBoostMultiplier*speedBoostMultiplierCount*currentMultiplier);
        float elapsed = 0f;

        while (elapsed < speedBoostDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        activeSpeedBoosts--;
        if (activeSpeedBoosts == 0)
        {
            speedBoostMultiplierCount = 0;
            forwardSpeed = originalSpeed;
            shipCollider.isTrigger = false;
            isSpeedBoosted = false;
            scoreManager.SetSpeedBoostMultiplier(currentMultiplier);
        }
    }

}
