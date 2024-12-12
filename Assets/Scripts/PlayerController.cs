using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public bool canjump = true;
    private bool isGrounded = true;
    private Rigidbody2D rb;
    public float jumpHeight; 
    public float jumpTime;
    public float jumpVelocity;
    public float terminalSpeed;
    public float coyoteTime;
    public float coyoteTimeCounter;
   
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
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            Debug.Log("openjump");
            canjump = true; 
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            Debug.Log("closejump");
            canjump = false; 
        }
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = new Vector2(playerInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = velocity;
        if (IsGrounded())
        {
            coyoteTimeCounter= coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
            if (playerInput.y > 0 && coyoteTimeCounter> 0 && canjump)
        {
            jumpVelocity = (2 * jumpHeight) / jumpTime;
            velocity = new Vector2(velocity.x, jumpVelocity);
            rb.gravityScale = 0f;
            rb.velocity = velocity;
            canjump = false;
        }

        if (!canjump)
        {
            rb.gravityScale = 2;

            if (velocity.y < terminalSpeed)
            {
                velocity = new Vector2(rb.velocity.x, terminalSpeed);
            }
            coyoteTimeCounter = 0;
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
