using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDetection : MonoBehaviour {

	//public Car2DController controller;
	public float motorReductionFactor;
	private Car2DController controller;

	// Use this for initialization
	void Start ()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			controller = GameObject.FindGameObjectWithTag("Player").GetComponent<Car2DController>();
			controller.ReduceMovement(motorReductionFactor);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			controller.ResetMovement();
		}
	}
}
