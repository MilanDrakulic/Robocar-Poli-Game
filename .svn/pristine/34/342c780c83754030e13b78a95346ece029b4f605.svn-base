using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {

	public Camera cameraToFollow;

	private List<Transform> movableBackgrounds;
	private float translationLength;
	private Vector3 previousViewportPosition;

	[SerializeField]
	private Vector3 currenViewportPosition;
	[SerializeField]
	private int visibleSpriteIndex = 0;
	[SerializeField]
	private Vector2 visibleSpritePosition;
	
	void Start ()
	{
		movableBackgrounds = new List<Transform>();
		//childrenTransforms = new List<Transform>();

		foreach (Transform childTransform in transform)
		{
			//AddBackgroundToList(childTransform);
			movableBackgrounds.Add(childTransform);
		}
		movableBackgrounds.Sort((a, b) => a.position.x.CompareTo(b.position.x));

		//childrenTransforms = gameObject.GetComponentsInChildren<Transform>();
		previousViewportPosition = cameraToFollow.WorldToViewportPoint(movableBackgrounds[0].position);
		currenViewportPosition = cameraToFollow.WorldToViewportPoint(movableBackgrounds[0].position);
		translationLength = movableBackgrounds[1].position.x - movableBackgrounds[0].position.x;

	}
	
	void Update ()
	{
		visibleSpritePosition = movableBackgrounds[visibleSpriteIndex].position;
		currenViewportPosition = cameraToFollow.WorldToViewportPoint(movableBackgrounds[visibleSpriteIndex].position);

		if ((previousViewportPosition.x > 0.5f) && (currenViewportPosition.x <= 0.5f))
		{
			TranslateRight();
		}
		else
			if ((previousViewportPosition.x < 0.5f) && (currenViewportPosition.x >= 0.5f))
			{
				TranslateLeft();
			}

		CheckCurrentVisibleSprite();
		previousViewportPosition = currenViewportPosition;
	}

	public void TranslateRight()
	{
		//Don't translate if "previous" member is already after the current (as on the beginning with two backgrounds)
		if (movableBackgrounds[visibleSpriteIndex].position.x < movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position.x)
			return;

		Vector3 newPosition = new Vector3(movableBackgrounds[visibleSpriteIndex].position.x + translationLength,
			movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position.y,
			movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position.z);

		movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position = newPosition;
		//movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position = GetReferencePoint(movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].BackgroundTransform);
	}

	public void TranslateLeft()
	{
		//Don't translate if "next" member is already before the current
		if (movableBackgrounds[visibleSpriteIndex].position.x > movableBackgrounds[GetNextMember(visibleSpriteIndex)].position.x)
			return;

		Vector3 newPosition = new Vector3(movableBackgrounds[visibleSpriteIndex].position.x - translationLength,
			movableBackgrounds[GetNextMember(visibleSpriteIndex)].position.y,
			movableBackgrounds[GetNextMember(visibleSpriteIndex)].position.z);
		movableBackgrounds[GetNextMember(visibleSpriteIndex)].position = newPosition;
		//movableBackgrounds[GetNextMember(visibleSpriteIndex)].position = GetReferencePoint(movableBackgrounds[GetNextMember(visibleSpriteIndex)].BackgroundTransform);
	}

	private int GetNextMember(int currentIndex)
	{
		if (currentIndex == movableBackgrounds.Count - 1)
			return 0;
		else
			return currentIndex+1;
	}

	private int GetPreviousMember(int currentIndex)
	{
		if (currentIndex == 0)
			return movableBackgrounds.Count - 1;
		else
			return currentIndex-1;
	}

	private void CheckCurrentVisibleSprite()
	{
		if ((cameraToFollow.WorldToViewportPoint(movableBackgrounds[visibleSpriteIndex].position).x > 0.5f)
			&& Mathf.Abs((cameraToFollow.WorldToViewportPoint(movableBackgrounds[visibleSpriteIndex].position).x - 0.5f)) > Mathf.Abs((cameraToFollow.WorldToViewportPoint(movableBackgrounds[GetPreviousMember(visibleSpriteIndex)].position).x - 0.5f)))
		{
			visibleSpriteIndex = GetPreviousMember(visibleSpriteIndex);
		}
		else
			if ((cameraToFollow.WorldToViewportPoint(movableBackgrounds[visibleSpriteIndex].position).x < 0.5f)
				&& Mathf.Abs((cameraToFollow.WorldToViewportPoint(movableBackgrounds[visibleSpriteIndex].position).x - 0.5f)) > Mathf.Abs((cameraToFollow.WorldToViewportPoint(movableBackgrounds[GetNextMember(visibleSpriteIndex)].position).x - 0.5f)))
			{
				visibleSpriteIndex = GetNextMember(visibleSpriteIndex);
			}
	}

	//private void AddBackgroundToList(Transform backgroundTransform)
	//{
	//	if (backgroundTransform.gameObject.tag != "MovableBackground")
	//		return;
	//	MovableBackground movableBackground = new MovableBackground()
	//	{
	//		BackgroundTransform = backgroundTransform,
	//		ReferencePoint = GetReferencePoint(backgroundTransform)
	//	};
	//	movableBackgrounds.Add(movableBackground);
	//}

	//private Vector2 GetReferencePoint(Transform spriteObjectTransform)
	//{
	//	return spriteObjectTransform.position;
	//	//SpriteRenderer spriteRenderer = spriteObjectTransform.gameObject.GetComponent<SpriteRenderer>();
	//	//return (spriteRenderer != null && spriteRenderer.sprite != null) ? spriteRenderer.sprite.pivot : Vector2.zero;
	//}
}
