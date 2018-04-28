using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public Transform[] backgrounds;
	public float smoothing = 1f;

	private Transform mainCamera;
	private Vector3 previousCameraPosition;

	private float parallaxScaleX;
	private float parallaxScaleY;
	private Vector3 targetPosition;

	private void Awake()
	{
		//mainCamera = Camera.main.transform;
	}

	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main.gameObject.transform;
		previousCameraPosition = mainCamera.position;	
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < backgrounds.Length; i++)
		{
			parallaxScaleX = (previousCameraPosition.x - mainCamera.position.x) * backgrounds[i].position.z * -1;
			parallaxScaleY = (previousCameraPosition.y - mainCamera.position.y) * backgrounds[i].position.z * -1;
			targetPosition = new Vector3(backgrounds[i].position.x + parallaxScaleX, backgrounds[i].position.y + parallaxScaleY, backgrounds[i].position.z);
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPosition, smoothing * Time.deltaTime);

			//parallaxScale = (previousCameraPosition.x - mainCamera.position.x) * backgrounds[i].position.z * -1;
			//targetPosition = new Vector3(backgrounds[i].position.x + parallaxScale, backgrounds[i].position.y, backgrounds[i].position.z);
			//backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPosition, smoothing * Time.deltaTime);

			previousCameraPosition = mainCamera.position;
		}
	}
}
