using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int collectibleScoreValue = 10;
    private ScoreManager scoreManager;
    [SerializeField]private ParticleSystem crashParticle;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Roadblock"))
        {
            StartShipCrashSequence();
            
            
            
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