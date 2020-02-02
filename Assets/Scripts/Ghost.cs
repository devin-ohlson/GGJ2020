using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
	[SerializeField] private float accel;
	[SerializeField] private float timeToBreak;
	private float breakTimer;
	[SerializeField] private float timeToTaunt;
	private float tauntTimer;
	[SerializeField] private float maxDistanceToGoal;

	[SerializeField] private float timeToBob;
	private bool bobbingUp;
	private float bobTimer;
	[SerializeField] private float bobForce;

	private Rigidbody2D rb;
	private AudioSource audioSource;
	private Animator animator;
	private List<Breakable> breakables;
	private Breakable breakableTarget;
	private SpriteRenderer spriteRenderer;
	private CharacterCtrl player;
	private State currentState;

	private enum State {
		MoveToBreak,
		Breaking,
		MoveToPlayer,
		Taunting
	}

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		breakables = new List<Breakable>(FindObjectsOfType<Breakable>());
		breakables = ShuffleBreakables(breakables);
		player = FindObjectOfType<CharacterCtrl>();
		StartMoveToBreakable();
	}

	//Uses Knuth shuffle algorithm
	private List<Breakable> ShuffleBreakables(List<Breakable> breakables) {
		List<Breakable> newList = new List<Breakable>(breakables);
		for (int i = 0; i < newList.Count; i++) {
			Breakable tmp = newList[i];
			int r = Random.Range(i, newList.Count);
			newList[i] = newList[r];
			newList[r] = tmp;
		}
		return newList;
	}

	private void FixedUpdate() {
		switch (currentState) {
			case State.MoveToBreak:
				MoveToTarget(breakableTarget.transform.position);
				CheckBreakableTarget();
				break;
			case State.Breaking:
				BreakBreakable();
				break;
			case State.MoveToPlayer:
				//Checking for player happens in OnTriggerEnter
				MoveToTarget(player.transform.position);
				break;
			case State.Taunting:
				TauntPlayer();
				break;
		}

		if (bobbingUp)
			rb.AddForce(Vector2.up * bobForce);
		else
			rb.AddForce(Vector2.up * -bobForce);
		bobTimer -= Time.deltaTime;
		if(bobTimer < 0) {
			bobTimer = timeToBob;
			bobbingUp = !bobbingUp;
		}
	}

	private void MoveToTarget(Vector2 targetPosition) {
		if (transform.position.x < targetPosition.x)
			spriteRenderer.flipX = true;
		else
			spriteRenderer.flipX = false;

		Vector2 angle = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
		rb.AddForce(accel * angle);
	}
	
	private void CheckBreakableTarget() {
		if (Vector2.Distance(transform.position, breakableTarget.transform.position) < maxDistanceToGoal) {
			currentState = State.Breaking;
			breakTimer = timeToBreak;
			animator.SetBool("Breaking", true);
			animator.SetBool("Idling", false);
		}
	}

	private void BreakBreakable() {
		breakTimer -= Time.deltaTime;
		if(breakTimer < 0) {
			breakableTarget.Break();
			audioSource.Play();
			if(Random.Range(0, 2) == 0)
				StartMoveToBreakable();
			else {
				currentState = State.MoveToPlayer;
				animator.SetBool("Breaking", false);
				animator.SetBool("Idling", true);
			}
		}
	}

	private void TauntPlayer() {
		tauntTimer -= Time.deltaTime;
		if(tauntTimer < 0) {
			StartMoveToBreakable();
		}
	}

	private void StartMoveToBreakable() {
		if (breakables.Count > 0) {
			breakableTarget = breakables[0];
			breakables.RemoveAt(0);

			animator.SetBool("Idle", true);
			animator.SetBool("Breaking", false);
			animator.SetBool("Taunting", false);
			currentState = State.MoveToBreak;
		}
		else {
			animator.SetBool("Idle", true);
			currentState = State.MoveToPlayer;
		}
	}

	//Used only to see if it's in the same room as the player
	private void OnTriggerEnter2D(Collider2D collision) {
		if(currentState == State.MoveToPlayer) {
			Room room = collision.gameObject.GetComponent<Room>();
			if(room != null) {
				if (room.CurrentRoom) {
					currentState = State.Taunting;
					tauntTimer = timeToTaunt;
					audioSource.Play();
					animator.SetBool("Taunting", true);
					animator.SetBool("Idle", false);
				}
			}
		}
	}

	//Objects call this when they get repaired to be added back to the queue
	public void ObjectRepaired(Breakable breakable) {
		breakables.Add(breakable);
	}
}