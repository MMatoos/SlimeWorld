using System.Collections;
using UnityEngine;

public class CharacterAttack2D : MonoBehaviour
{
    private float waitTime;
    public float startTime;
    
    public Transform attackPos;
    public LayerMask enemies;
    public float range;
    public int damage;
    public Animator animator;

    private void Update()
    {
        if (waitTime <= 0)
        {
            if (Input.GetKey("k"))
            {
                gameObject.GetComponent<CharacterMovement2D>().isAttacking = true;
                animator.SetTrigger("Attacking");
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPos.position, range, enemies);
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    enemiesInRange[i].GetComponent<EnemyController2D>().TakeDamage(damage/2);
                }
                waitTime = startTime;
                StartCoroutine(StopAttack(0.5f));
            }
            
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, range);
    }

    IEnumerator StopAttack(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<CharacterMovement2D>().isAttacking = false;
    }
}
