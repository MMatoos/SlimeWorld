using UnityEngine;

public class EnemyController2D : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float knockbackPower;
    [SerializeField] private HealthController healthController; 
    [SerializeField] private float knockbackDelay;
    [SerializeField] private GameObject blood;

    private CameraShake cameraShake;
    public int hp;
    private Rigidbody2D rb;
    private Transform target;

    private void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        healthController = GameObject.FindGameObjectWithTag("Health Controller").GetComponent<HealthController>();
        cameraShake = GameObject.FindGameObjectWithTag("Screen Shake").GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject, 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage();
            Instantiate(blood, other.transform.position, Quaternion.identity);
        }
    }

    void DealDamage()
    {
        healthController.playerHealth = healthController.playerHealth - damage;
        healthController.UpdateHealth();
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        cameraShake.Shake();
        Instantiate(blood, transform.position, Quaternion.identity);
        if(gameObject.GetComponent<EnemyPatrol2D>())
            gameObject.GetComponent<EnemyPatrol2D>().StopMoving();
    }
}
