using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int shootRoadblockScoreValue=5;
    public float speed = 20f;
    public float lifetime = 2f;
    private ScoreManager _scoreManager;
    [SerializeField]private ParticleSystem shootRoadblockParticle;
    

    void Start()
    {
        Destroy(gameObject, lifetime);
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Roadblock"))
        {
            PlayParticleEffect();
            Destroy(other.gameObject);
            Destroy(gameObject);
            _scoreManager.AddScore(shootRoadblockScoreValue);
        }
    }
    private void PlayParticleEffect()
    {
        if (shootRoadblockParticle != null)
        {
            ParticleSystem effect = Instantiate(shootRoadblockParticle, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}
