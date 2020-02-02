using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VisitorEmotion : MonoBehaviour
{
	private HappyUI ui;
	private VisitorMovement movement;
	private VisitorModel model;
	private Room currentRoom;
	private float happiness;
	
	private bool happinessFailed; // If happiness hits 0, visitor no longer increases happiness

	[SerializeField] private HappyUI uiPrefab = null;

	// Mostly set in stone items (all visitors should use the same value)
	[Tooltip("% Happiness increase per second from standing in a clean unbroken room.")]
	[SerializeField] private float happinessGrowthMod = 0.015f; // 1.5% happiness per second

	// Personality items (individual visitors)
	[Tooltip("% Extra happiness lost for each encounter with an unclean, broken room.")]
	[SerializeField] private float happinessDecayMod = 0;
	[Tooltip("Bonus score for 100% happiness when Visitor leaves (Note: 1 Score == 25 in-game points)")]
	[SerializeField] private int bonusHappyScore = 80; // 80 score * 25 points/score -> 2,000 points!

	[SerializeField] private Color badgeColor = Color.yellow;
	[SerializeField] private EmoteInput[] emotes = { };

	public int Score()
	{
		return Mathf.RoundToInt(bonusHappyScore * happiness);
	}

	public bool PerfectHappiness()
	{
		return (1 - 0.0001f) <= happiness;
	}

    void Start()
    {
		movement = GetComponent<VisitorMovement>();
		model = GetComponent<VisitorModel>();
		happiness = 0.5f; // Start at 50%
		happinessFailed = false;

		ui = GameObject.Instantiate<HappyUI>(uiPrefab);
		ui.Follow(transform);
    }
	
    void Update()
    {
		if (!happinessFailed)
		{
			// While current room is unbroken, increase happiness
			if (null != currentRoom && !currentRoom.IsBroken())
				ChangeHappiness(happinessGrowthMod * Time.deltaTime);

			// Update icon and bar for happiness
			UpdateUI();
		}
	}

	// VisitorMovement calls this when it enters a new room
	public void SearchRoom(Room newRoom)
	{
		currentRoom = newRoom;

		// Add up happiness loss for each unclean or broken item in the room
		int loss = 0;
		foreach (Breakable breakable in currentRoom.GetBreakables())
		{
			loss += BreakScore(breakable.Level());
		}

		// If any loss occurs, tell the model to act 
		if (loss > 0)
		{
			ChangeHappiness(- (1 + happinessDecayMod) * loss / 100.0f);
			UpdateUI();
		}
	}

	// Returns a integer percentage (100% => 100) for how much a break level counts for
	private int BreakScore(BreakableLevel level)
	{
		switch (level)
		{
			case BreakableLevel.CLICK:
				return 5;
			case BreakableLevel.TIMED:
				return 10;
			case BreakableLevel.MOTION:
				return 15;
			case BreakableLevel.PUZZLE:
				return 25;
		}
		return 0;
	}

	private void ChangeHappiness(float delta)
	{
		happiness = Mathf.Clamp(happiness + delta, 0, 1);

		if (0 == happiness)
		{
			movement.LeaveHouse();
			happinessFailed = true;
		}
	}

	private void UpdateUI()
	{
		Vector3 scale = ui.Bar.transform.localScale;
		scale.x = happiness;
		ui.Bar.transform.localScale = scale;

		Vector3 position = ui.Bar.transform.localPosition;
		position.x = scale.x / 2 - 0.5f;
		ui.Bar.transform.localPosition = position;

		int i = Mathf.Min((int)(happiness * emotes.Length), emotes.Length - 1);
		
		ui.Emote.sprite = emotes[i].sprite;
		ui.Bar.color = BarColor(happiness, i);
	}

	private Color BarColor(float happiness, int index)
	{
		if (emotes.Length > 1)
		{
			float lowHap, highHap;
			lowHap = 1.0f * index / emotes.Length;
			highHap = 1.0f * (index + 1) / emotes.Length;

			float t = (happiness - lowHap) / (highHap - lowHap);

			if (index == emotes.Length - 1)
				return Color.Lerp(emotes[index].color, badgeColor, t*t*t);
			else
				return Color.Lerp(emotes[index].color, emotes[index + 1].color, t);
		}
		else
		{
			return emotes[index].color;
		}
	}

	// Notes:
	//  - Call LeaveHouse() when happiness is 0
	//  - On finishing with an NPC (left either from 0 happiness or just time limit), reward bonus by score
	//     - On top of that, a happiness of 100% will award a Gold Happiness Badge! for that NPC (get all three for added challenge!!)

	[Serializable]
	internal class EmoteInput
	{
		[SerializeField] internal Sprite sprite = null;
		[SerializeField] internal Color color = Color.white;
	}
}
