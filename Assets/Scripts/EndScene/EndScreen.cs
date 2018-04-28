using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Invector.CharacterController;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace EMM_Integration
{

    public class EndScreen : MonoBehaviour {

        //[HideInInspector]
        public bool initialized;

        [Header("Configure End Screen")]
        public string nextScene;
        [Tooltip("Locks player Input when the end screen is open")]
        public bool lockInput;
        [Tooltip("Locks player Camera when the end screen is open")]
        public bool lockCamera;
        [Tooltip("You can have upto 5, however if you want more then adjust the container size!")]
        public EndScreenItemsConfiguration[] itemsConfiguration;

        [Header("UI References")]
        public GameObject EndScreenCanvas;
        public Text sceneNameText;
        public Transform endScreenItemsHolder;
        public GameObject endScreenItemPrefab;
		public Image highScoreImage;
		public AudioSource highScoreAudio;

		private bool shouldShowHighScore = false;

        // Use this for initialization
        void Start() {

        }

		public void ActivateEndScreenGameObject()
		{
			EndScreenCanvas.gameObject.SetActive(true);
			shouldShowHighScore = false;
			//highScoreImage.gameObject.SetActive(false);
		}

        public void generateItems() {

            //init
            initialized = true;

            ////lock player
            //FindObjectOfType<vMeleeCombatInput>().lockInput = lockInput;
            //FindObjectOfType<vMeleeCombatInput>().lockCamera = lockCamera;
            //FindObjectOfType<vThirdPersonController>().enabled = !lockInput;
            Time.timeScale = 0.00001f;


           Canvas[] allUI = FindObjectsOfType<Canvas>();
            //disable all UI
            for (int i = 0; i < allUI.Length; i++)
            {
                if (allUI[i].name != "Fader")
                    allUI[i].gameObject.SetActive(false);
            }

            //start end screen
            startGeneration();

        }

        void startGeneration() {

            //set text
            sceneNameText.text = SceneManager.GetActiveScene().name;

            // EndScreenCanvas.enabled = true;
            EndScreenCanvas.gameObject.SetActive(true);
			CheckHighScore();

            for (int i = 0; i < itemsConfiguration.Length; i++)
            {

                GameObject item = Instantiate(endScreenItemPrefab);
                item.transform.SetParent(endScreenItemsHolder);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.identity;

                //now set text
                item.GetComponent<EndScreenItem>().setEndScreenItem(itemsConfiguration[i].ItemName
                                    , itemsConfiguration[i].ItemValueInt, itemsConfiguration[i].ItemValueString);
            }
		}

        public void gotoNextScene() {

            //delete slot id
            PlayerPrefs.DeleteKey("slotLoaded_");

            //delete player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(player);

            //restore time scale (important)
            Time.timeScale = 1f;

            //load main menu
            PlayerPrefs.SetString("sceneToLoad", nextScene);

            //hide all menus
            Canvas[] allUI = FindObjectsOfType<Canvas>();
            //disable all UI
            for (int i = 0; i < allUI.Length; i++)
            {
                if (allUI[i].name != "Fader")
                    allUI[i].gameObject.SetActive(false);
            }

            //load level via fader
            FindObjectOfType<Fader>().FadeIntoLevel("LoadingScreen");
        }

		public void ShowHighScore()
		{
			shouldShowHighScore = true;
		}

		private void CheckHighScore()
		{
			if (shouldShowHighScore)
			{
				highScoreImage.gameObject.SetActive(true);
				//highScoreAudio.gameObject.SetActive(true);
				if (!highScoreAudio.isPlaying)
					highScoreAudio.Play();
			}
			else
			{
				highScoreImage.gameObject.SetActive(false);
			}
		}
    }

    [System.Serializable]
    public class EndScreenItemsConfiguration {
        [Tooltip("Your Item Name!")]
        public string ItemName;

        [Tooltip("If this Item Val is a string, use this field")]
        public string ItemValueString;

        [Tooltip("If this Item Val is an integer, use this field")]
        public int ItemValueInt;

    }
}