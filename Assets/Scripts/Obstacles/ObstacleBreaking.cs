﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBreaking : MonoBehaviour {
	public ParticleSystem breakingEffect;

	private bool hasCollidedWithPlayer = false;
	private List<GameObject> destructibles;
	private ControlConnectedTriggers controlConnectedTriggers;
	private AudioEffectsManager audioEffectsManager;

	// Use this for initialization
	void Start ()
	{
		controlConnectedTriggers = gameObject.GetComponentInParent<ControlConnectedTriggers>();
		audioEffectsManager = GameObject.FindObjectOfType<AudioEffectsManager>();
	}

	private void Awake()
	{
		breakingEffect.Stop();
		destructibles = new List<GameObject>();
		foreach (Transform child in transform)
		{
			if (child.tag == "Destructible")
				destructibles.Add(child.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.gameObject.tag == "Player") && (!hasCollidedWithPlayer))
		{
			if (IsPlayerInNitro(collision.gameObject))
			{
				breakingEffect.gameObject.SetActive(true);
				breakingEffect.Play();
				DisableDestructibles();
				if (controlConnectedTriggers != null)
					controlConnectedTriggers.DisableConnectedTriggers();
				if (audioEffectsManager != null)
					audioEffectsManager.SendMessage("PlayWoodBreaking");
			}
			GetComponent<BoxCollider2D>().enabled = false;
			hasCollidedWithPlayer = true;
		}
	}

	private bool IsPlayerInNitro(GameObject playerGameObject)
	{
		bool isInNitro = false;
		Car2DController playerController = playerGameObject.GetComponent<Car2DController>();
		if (playerController != null)
		{
			isInNitro = playerController.IsInNitro();
		}
		return isInNitro;
	}

	public void DisableDestructibles()
	{
		foreach (GameObject child in destructibles)
		{
			child.SetActive(false);
		}
	}
}
