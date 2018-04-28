using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EMM_Integration
{
    public class EndScreenTrigger : MonoBehaviour
    {
		private EndScreen endScreen;
		private Timer timer;
		private StarsCalculator starsCalculator;

		// Use this for initialization
		void Start()
        {
			starsCalculator = FindObjectOfType<StarsCalculator>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Player")
			{
				if (endScreen == null)
				{
					endScreen = FindObjectOfType<EndScreen>();
				}
				if (timer == null)
				{
					timer = FindObjectOfType<Timer>();
				}

				endScreen.ActivateEndScreenGameObject();
				if (starsCalculator == null)
				{
					starsCalculator = FindObjectOfType<StarsCalculator>();
				}

				timer.Stop();

				//Unfortunatelly, this code is dependent on order of items on EndScreen script
				endScreen.itemsConfiguration[0].ItemValueString = timer.timerLabel.text.ToString();
				int levelPoints = starsCalculator.CalculatePoints();

				if (endScreen.itemsConfiguration[1] != null)
					endScreen.itemsConfiguration[1].ItemValueInt = levelPoints;

				HandleHighScore(levelPoints);
				starsCalculator.DisplayStars();

				endScreen.generateItems();
			}
		}

		private void HandleHighScore(int levelPoints)
		{
			string activeSceneName = SceneManager.GetActiveScene().name;
			int levelHighScore = 0;
			if (PlayerPrefs.HasKey("HighScore_" + activeSceneName))
			{
				levelHighScore = PlayerPrefs.GetInt("HighScore_" + activeSceneName, 0);
			}
			if (levelPoints > levelHighScore)
			{
				PlayerPrefs.SetInt("HighScore_" + activeSceneName, levelPoints);
				levelHighScore = levelPoints;
				//TODO - add some visual for new high score
				endScreen.ShowHighScore();
			}
			if (endScreen.itemsConfiguration[2] != null)
				endScreen.itemsConfiguration[2].ItemValueInt = levelHighScore;
		}
    }
}