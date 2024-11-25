using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
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
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), 0);
        MovementUpdate(playerInput);
        
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = new Vector2(playerInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = velocity;
    }

    public bool IsWalking()
    {
        return  Mathf.Abs(rb.velocity.x) > 0.1f; 
    }
    public bool IsGrounded()
    {
        return true;
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
