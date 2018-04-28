using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDetection : MonoBehaviour
{
	//public Respawning respawningScript;
	private Respawning respawningScript;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			respawningScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Respawning>();
			respawningScript.InitiateBreaking();
		}
	}
}
