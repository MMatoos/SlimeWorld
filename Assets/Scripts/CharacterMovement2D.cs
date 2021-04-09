using UnityEngine;

public class CharacterMovement2D : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float speed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    public bool isAttacking;
    
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump") && !isAttacking)
            {
                jump = true;
                animator.SetBool("Jumping", true);
            }

            if (controller.m_Rigidbody2D.velocity.y > 2)
            {
                animator.SetBool("Jumping", true);
            }

            if (controller.m_Rigidbody2D.velocity.y == 0)
            {
                animator.SetBool("Jumping", false);
            }
        }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
        
    }

    public void Landing()
    {
        animator.SetBool("Jumping", false);
    }
    
}
