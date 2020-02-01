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

	public AudioClip breaking;
	public AudioClip repairing;
	public AudioClip repaired;

	private bool isInteracting;

	void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		interactCollider = GetComponent<Collider2D>();
		this.interactCollider.enabled = false;
		isInteracting = false;

		Break();
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
		// Do some animator thingy so that object is now in broken/repaired state?
		//animator.SetBool("isBroken", isBroken);
		interactCollider.enabled = isBroken; // Don't need to interact with repaired items
	}

	// Should be called on a successful repair
	protected void Repair()
	{
		SetBroken(false);
		RepairedSound();
	}

	public void Break()
	{
		SetBroken(true);
		BreakSound();

		// TODO: Popup on map/UI to show that break happened
	}

	public bool IsBroken()
	{
		return true;// animator.GetBool("isBroken");
	}

	// When interacted with, Breakables start repairing
	public virtual void Interact(CharacterCtrl controller)
	{
		if (!isInteracting)
		{
			isInteracting = true;
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

		isInteracting = false;
		return true;
	}
	
	protected abstract IEnumerator StartRepairing();
	protected abstract void StopRepairing();
	public abstract bool TryInteract(CharacterCtrl controller);
}
