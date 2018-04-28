using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenAndRotation : MonoBehaviour {
	public Animator rotationAnimator;
	public AudioSource sirenAudio;
	public string rotationButtonName;

	private bool rotationOn = false;
	
	// Update is called once per frame
	void Update ()
	{
		CheckRotation();
	}

	private void CheckRotation()
	{
		if (Input.GetButtonDown(rotationButtonName))
		{
			rotationOn = !rotationOn;
			rotationAnimator.SetBool("rotationOn", rotationOn);
			if (rotationOn)
			{
				sirenAudio.Play();
			}
			else
			{
				sirenAudio.Stop();
			}
		}
	}
}
