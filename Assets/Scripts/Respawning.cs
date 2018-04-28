using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawning : MonoBehaviour {
	public ParticleSystem smoke;
	public string respawnButtonName = "Fire3";
	//public GameObject prefabToInstantiate;

	private GameObject[] respawnPoints;
	private Rigidbody2D rigidBody;
	private bool isInBrokenAnimation = false;
	private Spawning spawningScript;
	private Car2DController controllerScript;

	void Start ()
	{
		respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
		Array.Sort(respawnPoints, (a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
		rigidBody = GetComponent<Rigidbody2D>();
		spawningScript = GameObject.Find("Spawner").GetComponent<Spawning>();
		//spawningScript = GameObject.FindObjectOfType<Spawning>();
		controllerScript = gameObject.GetComponent<Car2DController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckRespawn();
	}

	public void CheckRespawn()
	{
		if (Input.GetButtonDown(respawnButtonName))
		{
			Respawn();
		}
	}

	private void Respawn()
	{
		if (!isInBrokenAnimation)
		{
			if (smoke.isPlaying)
			{
				smoke.Stop();
			}

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

			spawningScript.Spawn(closestRespawn.transform.position, closestRespawn.transform.rotation);
			isInBrokenAnimation = false;
			Destroy(gameObject);
		}
	}

	private void OnJointBreak2D(Joint2D joint)
	{
		StartCoroutine(RespawnAfterBreak());
	}

	private IEnumerator RespawnAfterBreak()
	{
		if (!isInBrokenAnimation)
		{
			try
			{
				isInBrokenAnimation = true;
				controllerScript.enabled = false;
				if (smoke != null && !smoke.isPlaying)
				{
					smoke.gameObject.SetActive(true);
					smoke.Play(true);
				}

				yield return new WaitForSeconds(3);
				//UnityEditor.PrefabUtility.ResetToPrefabState(this.gameObject);
				
			}
			finally
			{
				controllerScript.enabled = true;
				isInBrokenAnimation = false;
				Respawn();
			}
		}
	}

	public void InitiateBreaking()
	{
		StartCoroutine(RespawnAfterBreak());
	}
}
