using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// Singleton to be easily accessible in a scene
	private static GameManager _manager;
	public static GameManager Instance() => _manager;

	private VisitorMovement visitor;
	[SerializeField] private VisitorMovement[] visitorPrefabs = { };
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private float delayBeforeDoorbell = 4;
	[SerializeField] private float delayAfterDoorbell = 1.5f;
	[SerializeField] private AudioSource doorbell = null;
	[SerializeField] private AudioSource badgeSmack = null;

	[SerializeField] private Image cover = null;
	[SerializeField] private Image badge = null;
	[SerializeField] private Text perfect = null;
	[SerializeField] private Text happiness = null;

	private int numberOfVisitors = 0;
	private float gameTime = 0;
	private float lastVisitor = 0;
	private int secondsBetweenVisitors = 60;

	private int score = 0; // This is the score from BOTH fixing the breakables AND from the visitor's reviews
	private int perfects = 0; // Stores a combination of 1, 10, and 100 for the three badges in order


	public Room startRoom;

    void Start()
    {
		_manager = this;
		doorbell.loop = false;
		badgeSmack.loop = false;
    }

	private bool visitorNotifiedToLeave = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene("MainMenu");
		}

		gameTime += Time.deltaTime;

		if (!visitorNotifiedToLeave && lastVisitor + secondsBetweenVisitors < gameTime)
		{
			if (visitor != null)
				visitor.FocusOnLeaving();
			else
				CompleteVisitor();
			visitorNotifiedToLeave = true;
		}
	}

	public void Repaired(BreakableLevel level)
	{
		int boost = 0;
		switch (level)
		{
			case BreakableLevel.CLICK:
				boost = 25;
				break;
			case BreakableLevel.TIMED:
				boost = 75;
				break;
			case BreakableLevel.MOTION:
				boost = 150;
				break;
			case BreakableLevel.PUZZLE:
				boost = 200;
				break;
		}
		score += boost;
	}

	// Called by visitor once it reaches door
	public void CompleteVisitor()
	{
		// Everything 
		if (visitor != null)
		{
			// Spawn score popup
			score += visitor.GetComponent<VisitorEmotion>().Score();

			// Check if perfect
			if (visitor.GetComponent<VisitorEmotion>().PerfectHappiness())
			{
				perfects += (int) Mathf.Pow(10, numberOfVisitors - 1);

				StartCoroutine(BadgeAnimation());
			}

			GameObject.Destroy(visitor.gameObject);
		}

		if (numberOfVisitors == visitorPrefabs.Length)
		{
			StartCoroutine(EndGame());
		}
		else
		{
			StartCoroutine(SpawnNextVisitor());
		}
	}

	private IEnumerator SpawnNextVisitor()
	{
		// Wait a delay before hittin' that bell
		yield return new WaitForSeconds(delayBeforeDoorbell);
		// Play doorbell
		doorbell.Play();
		// Wait some time
		yield return new WaitForSeconds(delayAfterDoorbell);
		// Spawn the next visitor at the door and start its timer

		visitor = GameObject.Instantiate<VisitorMovement>(visitorPrefabs[numberOfVisitors]);
		visitor.SetRoom(startRoom);
		if (spawnPoint != null) visitor.transform.position = spawnPoint.position;

		visitorNotifiedToLeave = false;

		numberOfVisitors++;
		lastVisitor = gameTime;
	}

	private IEnumerator EndGame()
	{
		// Save score
		PlayerPrefs.SetInt("Score", score);
		PlayerPrefs.SetInt("Perfects", perfects);

		yield return new WaitForSeconds(1);
		// Transition to score scene
		// Score scene should with restart and quit button or main menu
		SceneManager.LoadScene("ScoreScreen");
	}

	private IEnumerator BadgeAnimation()
	{
		float tmp = Time.timeScale;
		Time.timeScale = 0;

		float speed = 0.05f;

		cover.gameObject.SetActive(true);

		while (cover.color.a < 0.75 - 0.001)
		{
			Color color = cover.color;
			color.a += 3 * speed;
			cover.color = color;
			yield return new WaitForSecondsRealtime(speed);
		}

		perfect.gameObject.SetActive(true);
		happiness.gameObject.SetActive(true);
		badge.gameObject.SetActive(true);
		badgeSmack.Play();
		badge.color = Color.clear;
		Color text = perfect.color;
		text.a = 1;
		perfect.color = text;
		happiness.color = text;

		Vector2 vec = new Vector2(1, 0.2f);
		float radius = 5;
		while (radius > 0)
		{
			badge.transform.localPosition = radius * 50 * vec;

			badge.transform.localScale = Vector3.one * (2 * radius + 1);

			Color color = Color.white;
			color.a = Mathf.Min(5 - radius, 1);
			badge.color = color;

			radius -= 10 * speed;

			yield return new WaitForSecondsRealtime(speed / 2);
		}

		badge.transform.localPosition = Vector3.zero;
		badge.transform.localScale = Vector3.one;
		badge.color = Color.white;

		yield return new WaitForSecondsRealtime(2);
		
		while (cover.color.a > 0.001)
		{
			Color color = cover.color;
			color.a -= speed;
			cover.color = color;
			badge.color = color;

			color = perfect.color;
			color.a -= speed;
			perfect.color = color;
			happiness.color = color;
			yield return new WaitForSecondsRealtime(speed);
		}

		cover.gameObject.SetActive(false);
		badge.gameObject.SetActive(false);
		perfect.gameObject.SetActive(false);
		happiness.gameObject.SetActive(false);

		Time.timeScale = tmp;
	}
}
