using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour
{
	[SerializeField] private Text score;
	[SerializeField] private Image badge1;
	[SerializeField] private Image badge2;
	[SerializeField] private Image badge3;

	private void Start()
	{
		score.text = "" + PlayerPrefs.GetInt("Score");
		int perfects = PlayerPrefs.GetInt("Perfects");

		foreach (Image badge in new Image[]{ badge1, badge2, badge3 }) {
			badge.color = (perfects % 10 == 1 ? 1 : 0.4f) * Color.white;
			perfects = perfects / 10;
		}
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
