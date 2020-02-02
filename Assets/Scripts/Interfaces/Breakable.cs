using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Breakable : MonoBehaviour, Interactable
{
	private Animator animator;
	private AudioSource audioSource;
	private Collider2D interactCollider;
	protected SpriteRenderer spriteRenderer;
	public Room parentRoom;

	protected Ghost ghost;

	public AudioClip breaking;
	public AudioClip repairing;
	public AudioClip repaired;
	
	public Sprite fixedSprite, brokenSprite;

	protected bool IsInteracting { get; private set; }

	[SerializeField] private bool breakOnStart = false;
	protected virtual void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		interactCollider = GetComponent<Collider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		parentRoom = GetComponentInParent<Room>();
		ghost = FindObjectOfType<Ghost>();
		this.interactCollider.enabled = false;
		IsInteracting = false;

		if (breakOnStart) Break();
	}

	private void BreakSound()
	{
		audioSource.clip = breaking;
		audioSource.loop = false;
		audioSource.Play();
	}

	private void RepairingSound()
	{
		audioSource.clip = repairing;
		audioSource.loop = true;
		audioSource.Play();
	}

	private void RepairedSound()
	{
		audioSource.clip = repaired;
		audioSource.loop = false;
		audioSource.Play();
	}

	private void SetBroken(bool isBroken)
	{
		//Swap sprites
		interactCollider.enabled = isBroken; // Don't need to interact with repaired items
		if (isBroken)
			spriteRenderer.sprite = brokenSprite;
		else
			spriteRenderer.sprite = fixedSprite;
	}
	
	// Should be called on a successful repair
	protected virtual void Repair()
	{
		ghost.ObjectRepaired(this);
		IsInteracting = false;
		SetBroken(false);
		parentRoom.AdjustBroken(-1);
		RepairedSound();
	}

	virtual
	public void Break()
	{
		SetBroken(true);
		parentRoom.AdjustBroken(1);
		BreakSound();
		// TODO: Popup on map/UI to show that break happened
	}

	public bool IsBroken()
	{
		return true;
	}

	// When interacted with, Breakables start repairing
	public virtual void Interact(CharacterCtrl controller)
	{
		if (!IsInteracting)
		{
			IsInteracting = true;
			RepairingSound();
			StartCoroutine(StartRepairing());
		}
	}

	// When reset, Breakables stop their repairing sequence
	public bool Reset()
	{
		audioSource.Stop();
		StopCoroutine(StartRepairing());
		StopRepairing();

		IsInteracting = false;
		return true;
	}
	
	protected abstract IEnumerator StartRepairing();
	protected abstract void StopRepairing();
	public abstract bool TryInteract(CharacterCtrl controller);

	public abstract BreakableLevel Level();
}
