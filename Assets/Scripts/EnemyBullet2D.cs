using UnityEngine;

public class EnemyBullet2D : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private HealthController healthController;
    [SerializeField] private int damage;
    [SerializeField] private GameObject blood;
    
    private Rigidbody2D bulletRB;
    private GameObject target;

    private void Start()
    {
        healthController = GameObject.FindGameObjectWithTag("Health Controller").GetComponent<HealthController>();
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(direction.x, direction.y);
        Destroy(this.gameObject, 2);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage();
            Instantiate(blood, other.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Tilemap"))
        {
            Destroy(this.gameObject);
        }
    }

    void DealDamage()
    {
        healthController.playerHealth = healthController.playerHealth - damage;
        healthController.UpdateHealth();
    }
}
