using UnityEngine;

public class EnemyShooting2D : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletPlace;
    [SerializeField] private float delay;
    
    private float waitTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);

        if (distance < range && waitTime < Time.time)
        {
            Instantiate(bullet, bulletPlace.transform.position, Quaternion.identity);
            waitTime = delay + Time.time;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
