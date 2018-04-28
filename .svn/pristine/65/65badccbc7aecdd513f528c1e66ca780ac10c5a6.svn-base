using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlConnectedTriggers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisableConnectedTriggers()
	{
		foreach (BoxCollider2D collider in GetComponentsInChildren<BoxCollider2D>())
		{
			if (collider.isTrigger)
				collider.enabled = false;
		}
	}
}
