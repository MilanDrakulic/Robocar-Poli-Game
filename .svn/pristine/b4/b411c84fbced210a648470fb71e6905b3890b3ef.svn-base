using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroBaloon : MonoBehaviour {


	private NitroManager nitroManager;
	// Use this for initialization
	void Start () {
		nitroManager = GameObject.FindObjectOfType<NitroManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			nitroManager.AddNitro();
			//TODO - add animation for baloon popping
			nitroManager.PopABaloon();
			gameObject.SetActive(false); 
		}
	}
}
