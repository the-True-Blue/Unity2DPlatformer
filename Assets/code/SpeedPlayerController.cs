using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedPlayerController : MonoBehaviour
{
	public int speed = 10;
	public int jumpForce = 500;
	public int bounceForce = 500;
	public int spinDashSpeed = 250;
	public float homingDistance = 10f;
	public int homingSpeed = 600;
	public float stopDistance = 1f;

	public float slopeCheckDist = 0.6f;
	public float maxSlopeAngle = 60f;

	public string lastHit;

	public bool canControl = true;
	public bool canHit = true;

	private float slopeDownAngle;
	private float slopeSideAngle;
	private float lastSlopeAngle;
	private bool isOnSlope;
	private bool walkSlope;

	private float xSpeed;

	private Vector2 boxColliderSize;
	private Vector2 slopeNormPerp;

	BoxCollider2D box;
	Rigidbody2D body;
	Animator anim;

	public LayerMask groundLayer;
	public Transform feetTrans;
	bool grounded = false;
	float groundCheckDist = 0.6f;

	public float minSpinDashTime;

	public int playerSide = 0; //0 = left, 1 = right
	private bool spinDashing = false;
	public bool isHoming = false;
	public bool hit = false;
	private float spinDashTimer;
	public Vector2 startPos;


	public GameObject closeTarget = null;

	void Start()
	{
		box = GetComponent<BoxCollider2D>();
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		startPos = transform.position;
		boxColliderSize = box.size;
	}

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle(feetTrans.position, groundCheckDist, groundLayer);
		anim.SetBool("Grounded", grounded);
		if (canControl) { xSpeed = Input.GetAxisRaw("Horizontal") * speed; }
		else { xSpeed = 0; }
		
		anim.SetFloat("Speed", Mathf.Abs(xSpeed));

		SlopeCheck();

		if (xSpeed > 0 && transform.localScale.x < 0 || xSpeed < 0 && transform.localScale.x > 0)
		{
			transform.localScale *= new Vector2(-1, 1); //flip Sprite
			if (playerSide == 0) { playerSide = 1; }
			else if (playerSide == 1) { playerSide = 0; }
		}

		if (grounded && !isOnSlope || !spinDashing && !isHoming)
		{
			body.gravityScale = 1f;
			body.AddForce(transform.right * xSpeed * (speed/4)); //ground or air movement
		}
		else if (grounded && isOnSlope && walkSlope)
        {
			body.gravityScale = 1f;
			body.AddForce(new Vector2(slopeNormPerp.x * -xSpeed * (speed/4), slopeNormPerp.y * -xSpeed * (speed/4)));

		}
		else if (!grounded)
		{
			body.gravityScale = 1f;
			body.AddForce(transform.right * (xSpeed/2) * (speed / 2));
		}

	}

	void Update()
	{

		anim.SetBool("Spindashing", (spinDashing || (Mathf.Abs(speed) >= 50 && Mathf.Abs(speed) < 500)));
		anim.SetBool("Homing", isHoming);
		if (grounded || isOnSlope)
		{
			body.gravityScale = 9;
			isHoming = false;
			if (Input.GetButtonDown("Jump"))
			{
				body.AddForce(new Vector2(0, jumpForce * 1.1f));
				if (targetEnemy())
				{
					closeTarget.GetComponent<LockOn>().targeted = true;
				}
			}

			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetButton("Fire1"))
			{
				anim.SetBool("Charging", true);
				transform.GetChild(3).GetComponent<AudioSource>().Play(); //spinCharge
				spinDashTimer += Time.deltaTime;
			}
		}
		else if (!grounded)
        {
			body.gravityScale = 30;
        }


		if ((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || Input.GetButtonUp("Fire1")) || !grounded)
		{
			anim.SetBool("Charging", false);
			spinDashing = true;
			transform.GetChild(4).GetComponent<AudioSource>().Play(); //spinRelease
			if (spinDashTimer >= minSpinDashTime)
			{
				if (playerSide == 0)
				{
					body.AddForce(transform.right * spinDashSpeed * speed * 2);
				}
				else if (playerSide == 1)
				{
					body.AddForce((transform.right * -1) * spinDashSpeed * speed * 2);
				}
			}
			spinDashTimer = 0;
		}


		if (Input.GetButtonDown("Jump") && !grounded && !isHoming)
		{
			if (closeTarget != null)
			{
				var targetDir = (closeTarget.transform.position - transform.position).normalized;
				body.AddForce(targetDir * homingSpeed * 1.5f);
				transform.GetChild(2).GetComponent<AudioSource>().Play(); //homingAttack

				isHoming = true;
				canHit = false;
				anim.SetBool("Homing", isHoming);
			}
		}

		if (hit)
		{
			isHoming = false;
			canHit = true;
			if (lastHit == "Enemy") { body.AddForce(new Vector2(0, jumpForce / 2)); }
			else if (lastHit == "BouncePad") { body.AddForce(new Vector2(0, bounceForce * 2)); }
			
			closeTarget = null;
			if (targetEnemy())
			{
				body.velocity = Vector2.zero;
				closeTarget.GetComponent<LockOn>().targeted = true;
			}
			hit = false;
		}

	}

	public bool targetEnemy()
	{
		float homingTemp = homingDistance;

		Debug.Log("Looking For Enemies");
		GameObject[] holdTargets = GameObject.FindGameObjectsWithTag("Target");
		List<GameObject> targets = new List<GameObject>(holdTargets);
		Debug.Log(targets.Count);
		for (int i = 0; i < targets.Count; i++)
		{
			float tempDist = Vector3.Distance(targets[i].transform.position, transform.position);
			Debug.Log(tempDist);
			if (tempDist <= homingTemp)
			{
				homingTemp = tempDist;
				closeTarget = targets[i];
			}
		}
		homingTemp = homingDistance;


		if (closeTarget != null)
		{
			Debug.Log(closeTarget.name);
			return true;
		}
		return false;
	}
	
	private void SlopeCheck()
	{
		Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, boxColliderSize.y / 2));

		SlopeCheckHorz(checkPos);
		SlopeCheckVert(checkPos);
	}

	private void SlopeCheckHorz(Vector2 checkPos)
    {
		RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDist, groundLayer);
		RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDist, groundLayer);

		if (slopeHitFront)
		{
			isOnSlope = true;

			slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

		}
		else if (slopeHitBack)
		{
			isOnSlope = true;

			slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
		}
		else
		{
			slopeSideAngle = 0.0f;
			isOnSlope = false;
		}
	}

	private void SlopeCheckVert(Vector2 checkPos)
	{
		RaycastHit2D rayHit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDist, groundLayer);

		if (rayHit)
		{

			slopeNormPerp = Vector2.Perpendicular(rayHit.normal).normalized;

			slopeDownAngle = Vector2.Angle(rayHit.normal, Vector2.up);

			if (slopeDownAngle != lastSlopeAngle)
			{
				isOnSlope = true;
			}

			lastSlopeAngle = slopeDownAngle;

			Debug.DrawRay(rayHit.point, slopeNormPerp, Color.blue);
			Debug.DrawRay(rayHit.point, rayHit.normal, Color.green);

		}

		if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
		{
			walkSlope = false;
		}
		else
		{
			walkSlope = true;
		}
	}

}
