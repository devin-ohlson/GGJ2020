using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour
{
	[Range(0,1)]
	[SerializeField] private float masterVolume = 1;

	[SerializeField] private AudioSource mainTheme = null;
	[Range(0, 1)]
	[SerializeField] private float mainVolume = 1;

	[SerializeField] private AudioSource collegeBonus = null;
	[Range(0, 1)]
	[SerializeField] private float collegeVolume = 1;

	[SerializeField] private AudioSource hunterBonus = null;
	[Range(0, 1)]
	[SerializeField] private float hunterVolume = 1;

	[SerializeField] private AudioSource catsBonus = null;
	[Range(0, 1)]
	[SerializeField] private float catsVolume = 1;

	public int song; // Used for mixing, should be removed once done

	void Start()
	{
		mainTheme.loop = true;
		collegeBonus.loop = true;
		hunterBonus.loop = true;
		catsBonus.loop = true;

		StartMainTheme();

		// Used for mixing, should be removed once done
		switch (song)
		{
			case 1:
				PlayCollegeTheme();
				break;
			case 2:
				PlayHunterTheme();
				break;
			case 3:
				PlayCatsTheme();
				break;
			default:
				PlayMainTheme();
				break;
		}
	}

	void StartMainTheme()
	{
		mainTheme.Play();
		collegeBonus.Play();
		hunterBonus.Play();
		catsBonus.Play();

		PlayMainTheme();
	}

	private void SetAllMainVolumes(float main, float college, float hunter, float cats)
	{
		mainTheme.volume = masterVolume * main;
		collegeBonus.volume = masterVolume * college;
		hunterBonus.volume = masterVolume * hunter;
		catsBonus.volume = masterVolume * cats;
	}

	public void PlayMainTheme()
	{
		SetAllMainVolumes(mainVolume, 0, 0, 0);
	}

	public void PlayCollegeTheme()
	{
		SetAllMainVolumes(mainVolume, collegeVolume, 0, 0);
	}

	public void PlayHunterTheme()
	{
		SetAllMainVolumes(mainVolume, 0, hunterVolume, 0);
	}

	public void PlayCatsTheme()
	{
		SetAllMainVolumes(mainVolume, 0, 0, catsVolume);
	}
}
