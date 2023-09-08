using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool runBegun;
    [HideInInspector] public bool extraLife;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    private Animator animator;
    private bool canDoubleJump;
    [SerializeField] float doubleJumpForce;
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    private bool wallDetected;
    [SerializeField] float slideSpeed;
    [SerializeField] float slideTime;
    [SerializeField] float slideCooldown;
    private float slideTimeCounter;
    private bool isSliding;
    private float slideCooldownCounter;
    [SerializeField] float ceilingCheckDistance;
    private bool ceilingDetected;
    [SerializeField] float maxSpeed;
    [SerializeField] float speedMultiplier;
    [SerializeField] float milestoneIncreaser;
    [SerializeField] Vector2 knockBackDirection;
    private bool isKnocked;
    private bool canBeKnocked = true;
    private bool isDead;
    private float speedMilestone;
    private float defaultSpeed;
    private float defaultMilestoneIncrease;
    private int distanceIncreaser = 50;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncrease = milestoneIncreaser;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        AnimatorController();
        slideTimeCounter = slideTimeCounter - Time.deltaTime;
        slideCooldownCounter = slideCooldownCounter - Time.deltaTime;
        if(GameManager.instance.distance >= distanceIncreaser)
        {
            if (!extraLife)
            {
                extraLife= true;
            }
            distanceIncreaser += 50;
        }
        if (isDead)
        {
            return;
        }
        if(isKnocked) 
        { 
            return; 
        }
        if (runBegun)
        {
            Move();
        }
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        SpeedController();
        CheckForSlide();
        CheckInput();
    }
    
    public void Damage()
    {
        if(extraLife)
        {
            KnockBack();
            extraLife= false;
        }
        else
        {
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        AudioManager.instance.PlayAudio(4);
        isDead = true;
        canBeKnocked = false;
        playerRigidBody.velocity = knockBackDirection;
        animator.SetBool("isDead", isDead);
        Time.timeScale= .6f;
        yield return new WaitForSeconds(1f);
        playerRigidBody.velocity = new Vector2(0,0);
        GameManager.instance.GameEnded();
    }

    private IEnumerator Invincibility()
    {
        Color originalColor = spriteRenderer.color;
        Color darkenColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, .5f);  
        canBeKnocked = false;
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.15f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.15f);
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.25f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.25f);
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.3f);
        spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(.35f);
        spriteRenderer.color = darkenColor;
        yield return new WaitForSeconds(.4f);
        spriteRenderer.color = originalColor;
        canBeKnocked = true;
    }

    private void KnockBack()
    {
        if (!canBeKnocked) { return; }
        StartCoroutine(Invincibility());
        isKnocked = true;
        playerRigidBody.velocity = knockBackDirection;
    }

    private void CancelKnockBack()
    {
        isKnocked = false;
    }
    private void SpeedReset()
    {
        if(isSliding) 
        { 
            return; 
        }
        moveSpeed = defaultSpeed;
        milestoneIncreaser = defaultMilestoneIncrease;
    }
    private void SpeedController()
    {
        if (moveSpeed == maxSpeed)
        {
            return;
        }
        if(transform.position.x > speedMilestone)
        {
            speedMilestone = speedMilestone + milestoneIncreaser;
            moveSpeed = moveSpeed * speedMultiplier;
            milestoneIncreaser = milestoneIncreaser * speedMultiplier;
            if(moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }
    private void CheckForSlide()
    {
        if(slideTimeCounter < 0 && !ceilingDetected)
        {
            isSliding = false;
        }
    }

    private void Move()
    {
        if (wallDetected)
        {
            SpeedReset();
            return;
        }
        if (isSliding)
        {
            playerRigidBody.velocity = new Vector2(slideSpeed, playerRigidBody.velocity.y);
        }
        else
        {
            playerRigidBody.velocity = new Vector2(moveSpeed, playerRigidBody.velocity.y);
        }
    }

    private void AnimatorController()
    {
        animator.SetFloat("xVelocity", playerRigidBody.velocity.x);
        animator.SetFloat("yVelocity", playerRigidBody.velocity.y);
        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSliding", isSliding);
        animator.SetBool("isKnocked", isKnocked);
        if(playerRigidBody.velocity.y < -20)
        {
            animator.SetBool("canRoll", true);
        }
    }
    private void FinishRoll()
    {
        animator.SetBool("canRoll", false);
    }
    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, groundMask);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, groundMask);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide();
        }
    }

    private void Slide()
    {
        if(isGrounded && playerRigidBody.velocity.x != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void Jump()
    {
        if (isSliding)
        {
            return;
        }
        if (isGrounded)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
            AudioManager.instance.PlayAudio(Random.Range(1,2));
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            AudioManager.instance.PlayAudio(Random.Range(1, 2));
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, doubleJumpForce);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
