using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stumping : MonoBehaviour
{
	public float height;
	public float climbDuration;
	public float descendDuration;
	public float floatingDuration;

	private Sequence stump;
	private Tween climb;
	private Tween descend;
	private AudioEffectsManager audioEffectsManager;

	// Use this for initialization
	void Start ()
	{
		DOTween.Init();
		stump = DOTween.Sequence();
		stump.Append(transform.DOMoveY(height, climbDuration).SetEase(Ease.OutSine));
		stump.Append(transform.DOMoveY(transform.position.y, descendDuration).SetEase(Ease.OutBounce).SetDelay(floatingDuration));
		stump.SetLoops(-1);
		stump.Pause();
		audioEffectsManager = GameObject.FindObjectOfType<AudioEffectsManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnBecameVisible()
	{
		stump.Play();
	}

	private void OnBecameInvisible()
	{
		stump.Pause();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			if (audioEffectsManager != null)
				audioEffectsManager.SendMessage("PlayRockStump");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Ground")
		{
			if (audioEffectsManager != null)
				audioEffectsManager.SendMessage("PlayRockStump");
		}
	}
}
