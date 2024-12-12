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


    public float dashSpeed; 
    public float dashDuration;
    private bool isDashing = false;
    private bool turnleft;
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
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (!isDashing)
        {
            MovementUpdate(playerInput);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
          StartCoroutine(Dash(playerInput));
        }
       

    }
    private IEnumerator Dash(Vector2 playerInput)
    {
        isDashing = true;
        if (turnleft == true)
        {
            rb.velocity = new Vector2( -1 * dashSpeed, rb.velocity.y);
            Debug.Log("dash to left");
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
        }
        else if (turnleft == false)
        {
            rb.velocity = new Vector2(1 * dashSpeed, rb.velocity.y);
            Debug.Log("dash to right");
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
        }      
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            canjump = true; 
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
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
            turnleft = false;
            return FacingDirection.right;
        }
        else if (rb.velocity.x < -0.01f)
        {
            turnleft = true;
            return FacingDirection.left;
        }
        return FacingDirection.right;
    }
}
