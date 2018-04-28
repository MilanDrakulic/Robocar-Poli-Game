using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMM_Integration;
using System;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class StarsCalculator : MonoBehaviour {

	public float pointCalculationThresholdInSeconds;
	public int secondsToPointsRatio;
	public int[] starPointsThresholds;

	private EndScreen endScreenScript;
	private Timer timerScript;
	[SerializeField]
	private Image[] stars;

	// Use this for initialization
	void Start ()
	{
		endScreenScript = GameObject.FindObjectOfType<EndScreen>();
		timerScript = GameObject.FindObjectOfType<Timer>();
		InitializeStarsArray();
	}

	private void Awake()
	{
		endScreenScript = GameObject.FindObjectOfType<EndScreen>();
		timerScript = GameObject.FindObjectOfType<Timer>();
		InitializeStarsArray();
	}

	//A try at making a logic to dinamically adjust number of threshold when number of stars changes. Does not work because stars array is empty in edit mode. 
	// Update is called once per frame
	//void Update()
	//{
	//	if (Application.isEditor && !Application.isPlaying)
	//	{
	//		if (stars.Length == 0)
	//		{
	//			InitializeStarsArray();
	//		}

	//		if (stars.Length != starPointsThresholds.Length)
	//		{
	//			int[] bufferArray = new int[stars.Length];
	//			if (stars.Length > starPointsThresholds.Length)
	//			{
	//				starPointsThresholds.CopyTo(bufferArray, 0);
	//				starPointsThresholds = new int[stars.Length];
	//				bufferArray.CopyTo(starPointsThresholds, 0);
	//			}
	//			else
	//			{
	//				for (int i = 0; i < stars.Length; i++)
	//				{
	//					bufferArray[i] = starPointsThresholds[i];
	//				}
	//				starPointsThresholds = new int[stars.Length];
	//				bufferArray.CopyTo(starPointsThresholds, 0);
	//			}
	//		}
	//	}
	//}

	public int CalculatePoints()
	{
		int points = 0;
		float seconds = timerScript.GetSecondsTotal();
		if (seconds > pointCalculationThresholdInSeconds)
			return 0;

		points = (int)(pointCalculationThresholdInSeconds - seconds) * secondsToPointsRatio;
		return points;
	}

	public int CalculateStars()
	{
		int numberOfStarsEarned = 0;
		int points = CalculatePoints();
		for (int i = 0; i < starPointsThresholds.Length; i++)
		{
			if (starPointsThresholds[i] > points)
				break;
			else
				numberOfStarsEarned++;
		}
		return numberOfStarsEarned;
	}

	public void DisplayStars()
	{
		int numberOfStarsEarned = CalculateStars();

		if (numberOfStarsEarned > stars.Length)
			numberOfStarsEarned = stars.Length;

		for (int i = 0; i < numberOfStarsEarned; i++)
		{
			stars[i].color = Color.white;
			//stars[i].gameObject.GetComponent<Image>().color = Color.white;
		}

		for (int i = numberOfStarsEarned; i < stars.Length; i++)
		{
			stars[i].color = Color.clear;
			//stars[i].gameObject.GetComponent<Image>().color = Color.clear;
		}
	}

	private void InitializeStarsArray()
	{
		stars = gameObject.GetComponentsInChildren<Image>();
		Array.Sort(stars, (a, b) => a.name.CompareTo(b.name));
		//Array.Sort(stars, (a, b) => a.gameobject.transform.position.x.CompareTo(b.gameobject.transform.position.x));
	}
}
