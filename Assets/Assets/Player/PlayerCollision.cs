using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int destroyRoadblockScoreValue = 5;
    public int collectibleScoreValue = 10;
    private ScoreManager scoreManager;
    [SerializeField]private ParticleSystem crashParticle;
    [SerializeField] private ParticleSystem DestroyRoadblockCrashParticle;
    private PlayerMovement _playerMovement;
    

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Roadblock"))
        {
            if (_playerMovement.isSpeedBoosted)
            {
                DestroyRoadblockCrashParticle.Play();
                Destroy(collision.gameObject);
                scoreManager.AddScore(destroyRoadblockScoreValue);
            }
            else
            {
                StartShipCrashSequence(); 
            }
            
            
            
            
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            scoreManager.AddScore(collectibleScoreValue);
            Destroy(collision.gameObject);
        }
    }

    void GoToGameOverScreen()
    {
        SceneManager.LoadScene(2);
    }

    void StartShipCrashSequence()
    {
        crashParticle.Play();
        GetComponent<PlayerMovement>().enabled = false;
        Invoke("GoToGameOverScreen",2);
    }
   
}