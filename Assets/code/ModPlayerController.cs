using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModPlayerController : MonoBehaviour
{
    public int speed = 200;
    public int jumpForce = 500;
    Rigidbody2D rigidBody;
    Animator animator;

    public TextMeshProUGUI actionText;

    //int bulletForce = 100;
    public LayerMask groundLayer;
    public Transform feetTrans; //empty gameObject
    bool grounded = false;
    float groundCheckDist = 0.3f;
    //public GameObject bulletPrefab;

    [SerializeField] private float stickForce;
    [SerializeField] private float spinDashSpeed;
    [SerializeField] private float minSpinDashTime;
    [SerializeField] private float springLaunchVelocity;

    private bool flipped;
    private bool underWater;
    private bool spinDashing;
    private float spinDashTimer;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(feetTrans.position, groundCheckDist, groundLayer);
        //animator.SetBool("Grounded", grounded);

        float xSpeed = Input.GetAxis("Horizontal") * speed;
        //rigidBody.velocity = new Vector2(xSpeed, rigidBody.velocity.y);
        //animator.SetFloat("Speed", Mathf.Abs(xSpeed));


        if (xSpeed > 0 && transform.localScale.x < 0 || xSpeed < 0 && transform.localScale.x > 0)
        {
            transform.localScale *= new Vector2(-1, 1); //flip the sprite
                                                        //flipped = true;
        }
        // else { flipped = false; }


        //Spindash Controls
        //Charge


    }

    // Update is called once per frame
    void Update()
    {

        if (grounded && Input.GetButtonDown("Jump"))
        {
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }


        if (Input.GetKey(KeyCode.DownArrow))
        {
            spinDashing = true;
            spinDashTimer += Time.deltaTime;
            actionText.text = "Action: Charging SpinDash!";
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || !grounded)
        {
            spinDashing = false;
            if (spinDashTimer >= minSpinDashTime)
            {

                rigidBody.AddForce(transform.right * spinDashSpeed);
                actionText.text = "Action: Spindash!";
            }
            spinDashTimer = 0;
        }
    }
}
