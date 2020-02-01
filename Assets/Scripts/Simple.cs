using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Simple : MonoBehaviour, Breakable
{
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip breaking;
    public AudioClip repairing;

    void Awake(){
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void Interact(){
        
    }

    public virtual void Break(){

    }

    
}
