using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public Text timerLabel;
	private float time;
	private int minutes;
	private int seconds;
	private float fraction;

	private bool stopped = false;
	private bool reset;
	private float secondsTotal = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (stopped)
			return;

		time += Time.deltaTime;
		minutes = (int) (time / 60);
		seconds = (int) (time % 60);
		//practically, we are shifting decimal point in order to get the fraction
		fraction = (time * 100) % 99;

		timerLabel.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		secondsTotal = time;
	}

	public void Stop()
	{
		stopped = true;
	}

	private void Reset()
	{
		time = 0;
	}

	public float GetSecondsTotal()
	{
		return secondsTotal;
	}
}
