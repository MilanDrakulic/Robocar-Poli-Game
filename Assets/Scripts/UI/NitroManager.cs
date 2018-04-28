using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitroManager : MonoBehaviour {
	[Header("Nitro")]
	[Range(0, 10)]
	public int defaultNitroUses;
	[Range(0, 10)]
	public int maxNitroUses;
	public AudioSource baloonPop;

	private Image[] nitroImages;
	private int availableNitros;

	// Use this for initialization
	void Start ()
	{
		nitroImages = gameObject.GetComponentsInChildren<Image>(true);
		//Won't work well if there are more than 10 nitro images because of alphabetical sorting
		Array.Sort(nitroImages, (a, b) => a.name.CompareTo(b.name));
		if (defaultNitroUses > maxNitroUses)
			throw new Exception("Max number of nitro uses must be equal or greater than default number of nitro uses.");
		availableNitros = defaultNitroUses;
		SetNitros(availableNitros);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddNitro()
	{
		if (availableNitros < maxNitroUses)
			SetNitros(++availableNitros);
	}

	public void RemoveNitro()
	{
		if (availableNitros > 0)
			SetNitros(--availableNitros);
	}

	public void SetNitros(int numberOfActiveNitros)
	{
		//Available nitros in full color
		for (int i = 0; i < numberOfActiveNitros; i++)
		{
			nitroImages[i].color = Color.white;
			nitroImages[i].gameObject.GetComponent<CircleOutline>().effectColor = Color.yellow;
		}

		//Used nitros in gray color
		for (int i = numberOfActiveNitros; i < maxNitroUses; i++)
		{
			nitroImages[i].color = Color.gray;
			nitroImages[i].gameObject.GetComponent<CircleOutline>().effectColor = Color.clear;
		}

		//Unavailable nitros not visible
		for (int i = maxNitroUses; i < nitroImages.Length; i++)
		{
			nitroImages[i].color = Color.clear;
		}
	}

	public bool CanNitro()
	{
		if (availableNitros > 0)
			return true;
		else
			return false;
	}

	public void PopABaloon()
	{
		if (baloonPop != null && !baloonPop.isPlaying)
			baloonPop.Play();
	}
}
