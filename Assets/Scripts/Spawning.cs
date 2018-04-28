using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {
	public GameObject mainCharacterPrefab;
	public CinemachineVirtualCamera cameraToFollow;
	public GameObject[] respawnablePrefabs;
	//private Vector2 positionForSpawn;
	//private Quaternion rotationForSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(Vector2 position, Quaternion rotation)
	{
		GameObject spawnedGameObject = Instantiate(mainCharacterPrefab, position, rotation);
		cameraToFollow.Follow = spawnedGameObject.transform;


		foreach (GameObject respawnable in GameObject.FindGameObjectsWithTag("Respawnable"))
		{
			Destroy(respawnable);
		}

		foreach (GameObject respawnablePrefab in respawnablePrefabs)
		{
			Instantiate(respawnablePrefab);
		}

		//foreach (GameObject respawnable in GameObject.FindGameObjectsWithTag("Respawnable"))
		//{
		//	respawnable.SendMessage("Respawn");
		//}
	}
}
