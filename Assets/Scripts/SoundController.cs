using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
	
	public AudioSource source;
	public AudioClip hurt;
	public AudioClip death;
	public AudioClip fallDeath;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource> ();
	}

	public void playFallDeath() {
		source.PlayOneShot(fallDeath);
	}

	public void playDeath() {
		source.PlayOneShot(death);
	}

	public void playHurt() {
		source.PlayOneShot(hurt);
	}
}
