using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speedBoost;
    public float jumpSpeed;
    public Transform feet;
    public float boxWidth = 1f;
    public float boxHeight = 1f;
    public bool isGrounded = false;
    public LayerMask WhatIsGround;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isJumping = false;
    private bool canDoubleJump = false;
    private const int stateJump = 2;
    private const int stateIdle = 0;
    private const int stateRunning = 1;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        isGrounded = Physics2D.OverlapBox(
            feet.position,
            new Vector2(boxWidth, boxHeight),
            360.0f,
            WhatIsGround
            );

        float moveSpeed =
            Input.GetAxisRaw("Horizontal");
        if(moveSpeed != 0)
        {
            MoveHor(moveSpeed);
        }
        else
        {
            StopMoving();
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }	
        
        if(rb.velocity.y < 0)
        {
            showFalling();
        }
	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(feet.position,
            new Vector3(boxWidth, boxHeight, 0f));
    }

    private void showFalling()
    {
        anim.SetInteger("State", 3);
    }

    private void Jump()
    {
        //Debug.Log("Jump");
        if(isGrounded)
        {
            isJumping = true;
            rb.AddForce(new Vector2(0, jumpSpeed));
            anim.SetInteger("State", stateJump);

            canDoubleJump = true;
        }        
        else
        {
            if(canDoubleJump)
            {
                canDoubleJump = false;
                rb.velocity = 
                    new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0, jumpSpeed));
                anim.SetInteger("State", stateJump);
            }
        }
    }

    private void MoveHor(float speed)
    {
        if (speed > 0)
            sr.flipX = false;
        else if (speed < 0)
            sr.flipX = true;

        rb.velocity =
            new Vector2(speed * speedBoost, rb.velocity.y);
        if(!isJumping)
            anim.SetInteger("State", stateRunning);
    }

    private void StopMoving()
    {
        rb.velocity = 
            new Vector2(0, rb.velocity.y);
        if(isJumping == false)
            anim.SetInteger("State", stateIdle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isJumping = false;
    }
}
