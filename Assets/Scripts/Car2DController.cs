﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DriveType {AutoDrive, RearDrive, FrontDrive, AllWheelDrive}

public class Car2DController : MonoBehaviour
{
	[Header("Physics")]
	public Transform centerOfMass;
	public float jumpForce;
	public int maxRotationAngle;
	public float torqueMultiplier;
	public bool allowForceMidair;
	[Range(1, 4)]
	public int maxNumberOfJumps;
	public float nitroMultiplier;
	public float nitroDurationInSeconds;
	[Space]
	public DriveType driveType;
	public WheelJoint2D rearWheelJoint;
	public WheelJoint2D frontWheelJoint;
	public float motorMultiplier;

	[Space]
	[Header("Exhaust Settings")]
	public ParticleSystem exhaust;
	public float exhaustThreshold;
	public ParticleSystem nitroParticles;

	[Header("Ground Check")]
	public GameObject groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	[Header("Audio")]
	public AudioSource accelerationAudio;
	public AudioSource nitroAudio;

	[Header("Key config")]
	public string nitroKey;

	private bool shouldJump;
	private bool canJump = true;
	private int currentNumberOfJumps;

	private Rigidbody2D carRigidBody;
	private bool grounded;
	private bool canExhaust = true;
	private JointMotor2D rearWheelJointMotor;
	private JointMotor2D frontWheelJointMotor;
	private int motorForceSign = 1;
	private float previousMotorMultiplier;

	//Nitro
	private bool shouldNitro = false;
	private bool inNitro = false;
	private float lastNitroTime = 0;
	private NitroManager nitroManager;
	private bool canNitro = true;

	void Start ()
	{
		carRigidBody = GetComponent<Rigidbody2D>();
		carRigidBody.centerOfMass = centerOfMass.transform.localPosition;
		exhaust.Stop();
		nitroParticles.Stop();
		canExhaust = true;
		previousMotorMultiplier = motorMultiplier;

		SetWheelJoints();
		SetActiveJointMotors();

		var nitroParticleMain = nitroParticles.main;
		nitroParticleMain.duration = nitroDurationInSeconds;

		nitroManager = GameObject.FindObjectOfType<NitroManager>();

	}

	private void SetWheelJoints()
	{
		WheelJoint2D[] wheelJoints = GetComponents<WheelJoint2D>();
		if (wheelJoints.Length > 0)
		{
			rearWheelJoint = wheelJoints[0];
			motorForceSign = -1;
		}

		if (wheelJoints.Length > 1)
		{
			frontWheelJoint = wheelJoints[1];
		}
	}

	private void SetActiveJointMotors()
	{
		rearWheelJointMotor = rearWheelJoint.motor;
		frontWheelJointMotor = frontWheelJoint.motor;

		rearWheelJoint.useMotor = true;
		frontWheelJoint.useMotor = true;

		switch (driveType)
		{
			case DriveType.RearDrive:
				frontWheelJoint.useMotor = false;
				break;
			case DriveType.FrontDrive:
				rearWheelJoint.useMotor = false;
				break;
			default:
				break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		CheckJumping();
		CheckNitro();
	}

	public void FixedUpdate()
	{
		CheckExhaust();

		grounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, whatIsGround);
		float horizontalAxisInput = Input.GetAxis("Horizontal");

		if ((!shouldNitro)&&(Mathf.Abs(horizontalAxisInput) > 0.1f))
		{
			ApplyMotorForce(horizontalAxisInput);
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
			carRigidBody.AddTorque(torqueMultiplier * torqueSign, ForceMode2D.Impulse);
		}
	}

	private void CheckJumping()
	{
		if (canJump && Input.GetButtonDown("Jump"))
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
		if (canNitro && (!inNitro) && Input.GetButtonDown(nitroKey) && nitroManager.CanNitro())
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

	private void Jump()
	{
		carRigidBody.AddForce(Vector2.up * jumpForce);
		shouldJump = false;
		currentNumberOfJumps++;
		//animator.SetTrigger("jump");
	}

	private void ApplyMotorForce(float horizontalAxisInput)
	{
		//reverse = (horizontalAxisInput < -0.01);
		switch (driveType)
		{
			case DriveType.AutoDrive:
				ApplyAllWheelDriveMotorForce(1);
				break;
			case DriveType.RearDrive:
				ApplyRearDriveMotorForce(horizontalAxisInput);
				break;
			case DriveType.FrontDrive:
				ApplyFrontDriveMotorForce(horizontalAxisInput);
				break;
			case DriveType.AllWheelDrive:
				ApplyAllWheelDriveMotorForce(horizontalAxisInput);
				break;
		}
	}

	private void ApplyAllWheelDriveMotorForce(float horizontalAxisInput)
	{
		ApplyRearDriveMotorForce(horizontalAxisInput);
		ApplyFrontDriveMotorForce(horizontalAxisInput);
	}

	private void ApplyRearDriveMotorForce(float horizontalAxisInput)
	{
		if (rearWheelJoint != null)
		{
			rearWheelJointMotor.motorSpeed = horizontalAxisInput * motorMultiplier * motorForceSign;
			rearWheelJoint.motor = rearWheelJointMotor;
		}
	}

	private void ApplyFrontDriveMotorForce(float horizontalAxisInput)
	{
		if (frontWheelJoint != null)
		{
			frontWheelJointMotor.motorSpeed = horizontalAxisInput * motorMultiplier * motorForceSign;
			frontWheelJoint.motor = frontWheelJointMotor;
		}
	}

	private void ApplyNitro()
	{
		if (!nitroAudio.isPlaying)
		{
			nitroAudio.Play();
			nitroParticles.gameObject.SetActive(true);
			nitroParticles.Play();
		}

		ApplyMotorForce(nitroMultiplier);

		if (!inNitro)
		{
			lastNitroTime = Time.time;
			inNitro = true;
			nitroManager.RemoveNitro();
		}
		else
		{
			if ((lastNitroTime + nitroDurationInSeconds) <= Time.time)
			{
				shouldNitro = false;
				inNitro = false;
				float horizontalAxisInput = Input.GetAxis("Horizontal");
				ApplyMotorForce(horizontalAxisInput);
			}
		}
	}

	public bool IsInNitro()
	{
		return inNitro;
	}

	public void ReduceMovement(float motorReductionFactor, bool disableNitro = true, bool disableJump = true)
	{
		previousMotorMultiplier = motorMultiplier;
		motorMultiplier *= motorReductionFactor;
		canNitro = !disableNitro;
		canJump = !disableJump;
	}

	public void ResetMovement()
	{
		motorMultiplier = previousMotorMultiplier;
		canNitro = true;
		canJump = true;
	}
}
