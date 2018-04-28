using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2DNoJointsController : MonoBehaviour
{
	[Header("Physics")]
	public float maxSpeed;
	public float jumpForce;
	public float moveMultiplier;
	public int maxRotationAngle;
	public float torqueMultiplier;
	public bool allowForceMidair;
	[Range(1, 4)]
	public int maxNumberOfJumps;
	public float nitroForce;
	public float nitroDurationInSeconds;
	public Animator rotationAnimator;
	public float moveAnimationThreshold;
	[Space]
	[Header("Exhaust Settings")]
	public ParticleSystem exhaust;
	public float exhaustThreshold;

	[Header("Ground Check")]
	public GameObject groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	[Header("Audio")]
	public AudioSource accelerationAudio;
	public AudioSource nitroAudio;
	public AudioSource sirenAudio;

	private bool shouldJump;
	private int currentNumberOfJumps;

	private bool rotationOn = false;
	private Rigidbody2D poliRigidBody;
	private Animator animator;
	private bool grounded;
	private bool canExhaust = true;

	private bool reverse;
	//Nitro
	private bool shouldNitro = false;
	private bool inNitro = false;
	private float lastNitroTime = 0;
	private float move;

	private GameObject[] respawnPoints;

	// Use this for initialization
	void Start ()
	{
		this.poliRigidBody = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		exhaust.Stop();
		canExhaust = true;
		respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckJumping();
		CheckNitro();
		CheckRotation();
		CheckRespawn();
	}

	public void FixedUpdate()
	{
		CheckExhaust();

		grounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, whatIsGround);
		float horizontalAxisInput = Input.GetAxis("Horizontal");

		if ((!shouldNitro)&&(Mathf.Abs(horizontalAxisInput) > 0.1f))
		{
			ApplyHorizontalForce(horizontalAxisInput);
		}

		if (shouldNitro)
		{
			ApplyNitro();
		}

		if (shouldJump)
		{
			Jump();
		}

		if (!grounded)
		{
			ConstrainRotation();
		}

		ConstrainSpeed();
		UpdateMoveAnimation(horizontalAxisInput);
	}

	private void ConstrainRotation()
	{
		int modelAngle = (int)transform.rotation.eulerAngles.z % 360;
		int torqueSign = 0;
		if ((modelAngle > maxRotationAngle) && (modelAngle < 180))
		{
			torqueSign = -1;
		}
		else
		{
			if ((modelAngle > 180) && (modelAngle < (360 - maxRotationAngle)))
			{
				torqueSign = 1;
			}
		}

		if (torqueSign != 0)
		{
			poliRigidBody.AddTorque(torqueMultiplier * torqueSign, ForceMode2D.Impulse);
		}
	}

	private void ConstrainSpeed()
	{
		if (poliRigidBody.velocity.magnitude > maxSpeed)
		{
			Debug.Log("Velocity: " + poliRigidBody.velocity.magnitude.ToString());
			if (reverse)
				poliRigidBody.AddForce(Vector2.right * (poliRigidBody.velocity.magnitude - maxSpeed) * moveMultiplier);
			else
				poliRigidBody.AddForce(Vector2.left * (poliRigidBody.velocity.magnitude - maxSpeed) * moveMultiplier);
		}
	}

	private void CheckJumping()
	{
		if (Input.GetButtonDown("Jump"))
		{
			if (grounded)
			{
				shouldJump = true;
				currentNumberOfJumps = 0;
			}
			else
				if (currentNumberOfJumps < maxNumberOfJumps)
				{
					shouldJump = true;
				}
		}
	}

	private void CheckNitro()
	{
		if ((!inNitro) && Input.GetButtonDown("Fire1"))
		{
			shouldNitro = true;
		}
	}

	private void CheckExhaust()
	{
		if ((canExhaust) && (!exhaust.isPlaying) && (Input.GetAxis("Horizontal") > exhaustThreshold))
		{
			if ((grounded) || (!grounded && allowForceMidair))
			{
				exhaust.gameObject.SetActive(true);
				exhaust.Play();
				canExhaust = false;

				if (!accelerationAudio.isPlaying)
				{
					accelerationAudio.Play();
				}
			}
		}
		else
		{
			if (Input.GetAxis("Horizontal") <= exhaustThreshold)
			{
				canExhaust = true;
			}
		}
	}

	private void CheckRotation()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			rotationOn = !rotationOn;
			rotationAnimator.SetBool("rotationOn", rotationOn);
			if (rotationOn)
			{
				sirenAudio.Play();
			}
			else
			{
				sirenAudio.Stop();
			}
		}
	}

	public void CheckRespawn()
	{
		if (Input.GetButtonDown("Fire3"))
		{
			GameObject closestRespawn = respawnPoints[0];
			foreach (GameObject respawnPoint in respawnPoints)
			{
				if ((transform.position.x - respawnPoint.transform.position.x) > 0)
				{
					closestRespawn = respawnPoint;
				}
				else
				{
					break;
				}
			}
			transform.position = closestRespawn.transform.position;
			transform.rotation = closestRespawn.transform.rotation;
			poliRigidBody.velocity = Vector2.zero;
		}
	}

	private void Jump()
	{
		poliRigidBody.AddForce(Vector2.up * jumpForce);
		shouldJump = false;
		currentNumberOfJumps++;
		//animator.SetTrigger("jump");
	}

	private void ApplyHorizontalForce(float horizontalAxisInput)
	{
		if (horizontalAxisInput < -0.01)
		{
			if (reverse == true)
				animator.SetBool("reverse", true);
			reverse = true;
		}
		else
		{
			if (reverse == false)
				animator.SetBool("reverse", false);
			reverse = false;
		}

		if (allowForceMidair)
		{
			poliRigidBody.AddForce(Vector2.right * horizontalAxisInput * moveMultiplier, ForceMode2D.Force);
		}
		else
		{
			if (grounded)
			{
				poliRigidBody.AddForce(Vector2.right * horizontalAxisInput * moveMultiplier, ForceMode2D.Force);
			}
		}

	}

	private void ApplyNitro()
	{
		if (!nitroAudio.isPlaying)
		{
			nitroAudio.Play();
		}
		poliRigidBody.AddForce(Vector2.right * nitroForce, ForceMode2D.Force);
		if (!inNitro)
		{
			lastNitroTime = Time.time;
			inNitro = true;
		}
		else
		{
			if ((lastNitroTime + nitroDurationInSeconds) <= Time.time)
			{
				shouldNitro = false;
				inNitro = false;
			}
		}
	}

	private void UpdateMoveAnimation(float horizontalAxisInput)
	{
		move = (poliRigidBody.velocity.magnitude * 2/ maxSpeed) * (reverse ? -1 : 1);

		//if (Mathf.Abs(move) < moveAnimationThreshold)
		//	move = 0;
		animator.SetFloat("move", move);
	}
}
