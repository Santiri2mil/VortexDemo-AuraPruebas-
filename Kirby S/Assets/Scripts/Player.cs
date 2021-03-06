﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeleft;
    private float lastDash=-100f;

    private int amountOfJumpsLeft;

    public bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isAttempingToJump;
    private bool CheckJumpMult;
    private bool canMove = true;
    private bool canFlip=true;
    private bool isTouchingLedge;
    private bool canClimbledge=false;
    private bool ledgeDetected;
    private bool isDashing;
    private bool isinPlataform;


    private Rigidbody2D rb;
    private Animator anim;


    public int amountOfJump=1;
    private int facingDirection=-1;
    public int amountofDash=1;
    public int amountofDashLeft;

    public float MoveSpeed = 10.0f;
    public float jumpForce  = 16.0f;
    public float groundcheckRadius;
    public float wallCheckDistance;
    public float wallSlidespeed;
    public float MovementForceAir;
    public float arirDragMult=0.95f;
    public float JUMPHigthMult=0.5f;
    public float wallJumpForce;
    public float jumpTimerSet=0.15f;
    public float TurnTimerSet = 0.1f;
    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float DashTime;
    public float DashSpeed;
    public float dashCooldown;
    


    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    public Vector2 WallJumpDirection;

    public ParticleSystem Dust;

    public Transform groundCheck;
    public Transform WallCheck;
    public Transform ledgeCheck;
   


    public LayerMask whatIsGround;
    public LayerMask isaPlataform;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJump;
        amountofDashLeft = amountofDash;
        WallJumpDirection.Normalize();//Normalize hace que el vector valga 1
      
    }
    void Update()//Ejecuta los metodos mientras corre
    {
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        ChechDash(xRaw,yRaw);
        
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
        
    }
    private void CheckSurroundings()//Funciones que Detectan los alrdedores
    {
       isinPlataform = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, isaPlataform);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundcheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);
        if(isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = WallCheck.position;
        }
     
        
    }
    private void CheckIfWallSliding()//logica para permitir el deslice de pared
    {
        if (isTouchingWall && movementInputDirection==facingDirection&& rb.velocity.y <0 && !canClimbledge)
        {
            isWallSliding = true;
            //canMove = true;
            //canFlip=true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbledge&&!isinPlataform)
        {
            canClimbledge = true;
            if(isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            canMove = false;
            canFlip = false;
            anim.SetBool("CanClimbLedge", canClimbledge);
            
        }
        if(canClimbledge)
        {
            transform.position = ledgePos1;
        }
    }
    public void FinishLedgeClimb()
    {
        canClimbledge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("CanClimbLedge", canClimbledge);
    }
   
    private void CheckIfCanJump()//permite brincar
    {
        if(isGrounded && rb.velocity.y<=0.01f)
        {
            amountOfJumpsLeft = amountOfJump;
        }
        if(isTouchingWall)
        {
            canWallJump = true;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }
    private void CheckMovementDirection()//Revisa la direccion del jugador
    {
        if (isFacingRight && movementInputDirection<0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x)>=0.001f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void UpdateAnimations()//Conectar condciones con animacion
    {
        anim.SetBool("Is Walking", isWalking);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("IsWallSlide", isWallSliding);
        //anim.SetBool("CanClimbLedge", canClimbledge);
    }
    private void CheckInput()//Funcion que revisa los input del jugador
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded||(amountOfJumpsLeft>0&&isTouchingWall))
            {
                NormalJump();
                if (isGrounded)
                {
                    createDust();
                }
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttempingToJump = true;
            }
        }
        if(Input.GetButtonDown("Horizontal")&& isTouchingWall)
        {
            if(!isGrounded && movementInputDirection !=facingDirection)
            {
                canMove = false;
                canFlip = false;
                turnTimer = TurnTimerSet;
            }
            else if(isGrounded||isinPlataform && isTouchingWall)
            {
                canMove = true;
                canFlip = true;
            }
        }
        if(turnTimer>=0)
        {
            turnTimer = Time.deltaTime;
            if(turnTimer<=0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if(CheckJumpMult&& !Input.GetButton("Jump"))//Altura de brinco
        {
            CheckJumpMult = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * JUMPHigthMult);
        }
        if(isGrounded)
        {
            amountofDashLeft = 1;
        }
        if(Input.GetButtonDown("Dash"))
        {
            if(Time.time>=(lastDash+dashCooldown)&&amountofDashLeft>0)
            {
                AttemptToDash();
                createDust();
            }
            
        }
       
    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeleft = DashTime;
        lastDash = Time.time;
        if(!isGrounded)
        {
            amountofDashLeft--;
        }
    }
    private void ChechDash(float x,float y)
    {
        if(dashTimeleft>0)
        {
            if (isDashing)
            {
                canFlip = false;
                canMove = false;

                rb.velocity = Vector2.zero;
                Vector2 dir = new Vector2(x, y);
                
                rb.velocity += dir.normalized * DashSpeed;


                dashTimeleft -= Time.deltaTime;
            }
        }
        if(dashTimeleft<=0||isTouchingWall)
        {
            isDashing = false;
            canMove = true;
            canFlip = true;

        }
    }
    private void CheckJump()//Logica de brinco
    {
       if(jumpTimer>0)
       {
            if(!isGrounded && isTouchingWall && movementInputDirection!=0 )
            {
                WallJump();
            }
            else if(isGrounded || amountOfJumpsLeft > 0)
            {
                NormalJump();
                
            }
       }
       if(isAttempingToJump )
       {
            jumpTimer -= Time.deltaTime;
       }
    }
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttempingToJump = false;
            CheckJumpMult = true;
            canFlip = true;
            canMove = true;
            if(!isGrounded)
            {
                amountOfJumpsLeft--;
            }
        }
    }
    private void WallJump()
    {
        if (canWallJump)//WallJump
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJump;
            amountOfJumpsLeft--;
            Vector2 forceAdd2 = new Vector2(wallJumpForce * WallJumpDirection.x * movementInputDirection, wallJumpForce * WallJumpDirection.y);
            rb.AddForce(forceAdd2, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttempingToJump = false;
            CheckJumpMult = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }
    }
    private void ApplyMovement()//Aplicar el movimiento 
    {
        
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)//Fuerza de Cambio de direccion en el aire
        {
            rb.velocity = new Vector2(rb.velocity.x * arirDragMult, rb.velocity.y);
            
        }
        if (canMove)//Caminar
        {
            rb.velocity = new Vector2(MoveSpeed * movementInputDirection, rb.velocity.y);
        }
        //Wall Sliding
        if(isWallSliding)//movimiento de deslice
        {
            if(rb.velocity.y < -wallSlidespeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidespeed);
            }
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    public void DisableMove()
    {
        canMove = false;
    }
    public void EnableMove()
    {
        canMove = true;
    }
    //Habilitar movimiento del jugador
    private void Flip()//Voltea los sprites
    {
        if(!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            if(isGrounded)
            {
                createDust();
            }
            
        }
        
    }

    
    private void OnDrawGizmos()//Dibuja los detectores
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundcheckRadius);
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataform")
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
            isGrounded = true;
            isinPlataform = true;
            transform.parent = collision.transform;
            
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataform")
        {
            isinPlataform = true;
            isGrounded = true;
            transform.parent = collision.transform;
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plataform")
        {
            isinPlataform = false;
            transform.parent = null;
        }
    }
    void createDust ()
    {
        Dust.Play();
    }
    public int F_Direction()

    {
        return facingDirection;
    }

}
