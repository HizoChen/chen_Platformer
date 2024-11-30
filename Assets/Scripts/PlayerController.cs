using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isGrounded = true;
    private Rigidbody2D rb;
    public float jumpHeight; 
    public float jumpTime;
    public float jumpVelocity;
    public float terminalSpeed;

    public enum FacingDirection
    {
        left, right
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MovementUpdate(playerInput);
        
    }
    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = new Vector2(playerInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = velocity;


        if (playerInput.y > 0 && IsGrounded())
        {
            jumpVelocity = (2 * jumpHeight) / jumpTime;
            velocity = new Vector2(velocity.x, playerInput.y * jumpVelocity);
            rb.gravityScale = 0f;
            rb.velocity = velocity;
        }

        if (playerInput.y <= 0)
        {
            rb.gravityScale = 1;

            if (velocity.y < terminalSpeed)
            {
                velocity = new Vector2(rb.velocity.x, terminalSpeed);
                Debug.Log(" Terminal Velocity");
            }
        
          }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }


    public bool IsWalking()
    {
        return  Mathf.Abs(rb.velocity.x) > 0.1f; 
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public FacingDirection GetFacingDirection()
    {
        if (rb.velocity.x > 0.01f)
        {
            return FacingDirection.right;
        }
        else if (rb.velocity.x < -0.01f)
        {
            return FacingDirection.left;
        }
        return FacingDirection.left;
    }
}
