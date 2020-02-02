using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowObject))]
public class HappyUI : MonoBehaviour
{
	[SerializeField] private SpriteRenderer happyBar = null;
	public SpriteRenderer Bar { get => happyBar; }
	[SerializeField] private SpriteRenderer barEmote = null;
	public SpriteRenderer Emote { get => barEmote; }

	private FollowObject _follower;
	private FollowObject Follower
	{
		get
		{
			if (_follower == null)
			{
				_follower = GetComponent<FollowObject>();
			}
			return _follower;
		}
	}
	public void Follow (Transform transform)
	{
		Follower.toFollow = transform;
	}
}
