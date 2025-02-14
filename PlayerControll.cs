using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
  private Rigidbody2D rb;
  public Animator anim;
    private float HorizontalMove = 0f;
    private bool FacingRight = true;

    [Header("Player Movement Settings")]
    [Range(0, 10f)] public float speed = 1f;
    public float jumpForce = 8f;
     public float dashDistance = 5f; 
    public float dashDuration = 0.2f; 
    public float dashSmoothTime = 0.1f; 
    private bool isDashing = false; 
    private bool canDash = false; 
    private Vector3 targetPosition; 
    private Vector3 velocity = Vector3.zero;

    [Header("Jump Settings")]
    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius;
    public LayerMask Ground;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            anim.StopPlayback();
            anim.Play("dash");

            Vector3 dashDirection = Vector3.zero;

            if (transform.localScale.x > 0) 
            {
                dashDirection = Vector3.right * dashDistance;
            }
            else 
            {
                dashDirection = Vector3.left * dashDistance;
            }

            StartCoroutine(Dash(dashDirection));
        }

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        HorizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (HorizontalMove < 0 && FacingRight)
        {
            Flip();
        }
        else if (HorizontalMove > 0 && !FacingRight)
        {
            Flip();
        }

    }
    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(HorizontalMove * 10f, rb.velocity.y);
        rb.velocity = targetVelocity;

        anim.SetFloat("moveX" , Mathf.Abs(HorizontalMove));

        CheckingGround();
    }

    private void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);

        canDash = true;
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;

        Vector3 originalPosition = transform.position;
        targetPosition = originalPosition + direction;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dashSmoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        canDash = false;
    }
    
}