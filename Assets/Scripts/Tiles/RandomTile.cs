using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace UnityEngine.Tilemaps
{
	[Serializable]
	public class RandomTile : Tile
	{
		[SerializeField]
		public bool positionLockedSpriteRandomness;
		[SerializeField]
		public bool positionLockedHeightRandomness;
		[SerializeField]
		public bool varyPosition;
		[SerializeField]
		public float positionVariationAmplitude;

		[SerializeField]
		public Sprite[] m_Sprites;

		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			base.GetTileData(location, tileMap, ref tileData);
			if ((m_Sprites != null) && (m_Sprites.Length > 0))
			{
				long hash;
				long heightHash;
				if (positionLockedSpriteRandomness)
					hash = location.x;
				else
					hash = DateTime.Now.Millisecond;
				if (positionLockedHeightRandomness)
					heightHash = location.x;
				else
					heightHash = DateTime.Now.Millisecond;

				Random.InitState((int)HashMumboJumbo(hash, location));
				tileData.sprite = m_Sprites[(int) (m_Sprites.Length * Random.value)];
				if (varyPosition)
				{
					if (positionLockedSpriteRandomness != positionLockedHeightRandomness)
					{
						Random.InitState((int)HashMumboJumbo(heightHash, location));
					}

					tileData.flags = TileFlags.LockTransform;
					//looks better if we only vary height downwards
					int sign = -1;
					//int sign = (int)((Random.value * 1000) % 2);
					//if (sign == 0)
					//	sign = -1;
					Vector3 tilePosition = tileData.transform.GetColumn(3);
					tilePosition.y += sign * (Random.value * positionVariationAmplitude);
					Matrix4x4 tileMatrix = Matrix4x4.TRS(tilePosition, tileData.transform.rotation, Vector3.one);
					tileData.transform = tileMatrix;
				}
			}
		}

		private long HashMumboJumbo(long hash, Vector3Int location)
		{
			hash = (hash + 0xabcd1234) + (hash << 15);
			hash = (hash + 0x0987efab) ^ (hash >> 11);
			hash ^= location.y;
			hash = (hash + 0x46ac12fd) + (hash << 7);
			hash = (hash + 0xbe9730af) ^ (hash << 11);
			return hash;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/Random Tile")]
		public static void CreateRandomTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Random Tile", "New Random Tile", "asset", "Save Random Tile", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RandomTile>(), path);
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(RandomTile))]
	public class RandomTileEditor : Editor
	{
		SerializedProperty positionLockedSpriteRandomness;
		SerializedProperty positionLockedHeightRandomness;
		SerializedProperty varyPosition;
		SerializedProperty positionVariationAmplitude;

		private RandomTile tile { get { return (target as RandomTile); } }

		private void OnEnable()
		{
			positionLockedSpriteRandomness = serializedObject.FindProperty("positionLockedSpriteRandomness");
			positionLockedHeightRandomness = serializedObject.FindProperty("positionLockedHeightRandomness");
			varyPosition = serializedObject.FindProperty("varyPosition");
			positionVariationAmplitude = serializedObject.FindProperty("positionVariationAmplitude");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(positionLockedSpriteRandomness);
			EditorGUILayout.PropertyField(positionLockedHeightRandomness);
			EditorGUILayout.PropertyField(varyPosition);
			EditorGUILayout.PropertyField(positionVariationAmplitude);
			tile.varyPosition = varyPosition.boolValue;
			tile.positionVariationAmplitude = positionVariationAmplitude.floatValue;
			serializedObject.ApplyModifiedProperties();

			int count = EditorGUILayout.DelayedIntField("Number of Sprites", tile.m_Sprites != null ? tile.m_Sprites.Length : 0);
			if (count < 0)
				count = 0;
			if (tile.m_Sprites == null || tile.m_Sprites.Length != count)
			{
				Array.Resize<Sprite>(ref tile.m_Sprites, count);
			}

			if (count == 0)
				return;

			EditorGUILayout.LabelField("Place random sprites.");
			EditorGUILayout.Space();

			for (int i = 0; i < count; i++)
			{
				tile.m_Sprites[i] = (Sprite) EditorGUILayout.ObjectField("Sprite " + (i+1), tile.m_Sprites[i], typeof(Sprite), false, null);
			}		
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);
		}
	}
#endif
}
