using UnityEngine;

public class EnemyPatrol2D : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float startSnaredTime;
    private float snaredTime;
    private float savedSpeed;

    private bool right = true;
    private float dir = 1f;
    private RaycastHit2D hit;


    private void Start()
    {
        savedSpeed = speed;
    }

    private void FixedUpdate()
    {
        CheckCollision();
        ChangeDirection();
    }

    private void CheckCollision()
    {
        hit = Physics2D.Raycast(groundChecker.position, transform.right*dir, 0.2f, ground);
        if (snaredTime <= 0)
        {
            speed = savedSpeed;
        }
        else
        {
            speed = 0;
            snaredTime -= Time.deltaTime;
        }
    }

    private void ChangeDirection()
    {
        if (hit.collider == false)
        {
            if (right)
            {
                rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
            }
            else
            {
                rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
            }
        }
        else
        {
            right = !right;
            dir *= -1;
            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
        }
    }

    public void StopMoving()
    {
        snaredTime = startSnaredTime;
    }
}
