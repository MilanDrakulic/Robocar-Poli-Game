using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSlowdown : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Car2DController controllerScript = collision.gameObject.GetComponent<Car2DController>();
			controllerScript.enabled = false;
		}
	}
}
