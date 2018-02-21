using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
	
	public AudioSource source;
	public AudioClip backgroundMusic;
	public AudioClip hurt;
	public AudioClip death;
	public AudioClip fallDeath;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource> ();
		playBackgroundMusic ();
	}

	public void playBackgroundMusic() {
		source.loop = true;
		source.PlayOneShot(backgroundMusic);
	}

	public void playFallDeath() {
		source.loop = false;
		source.PlayOneShot(fallDeath);
		source.loop = true;
	}

	public void playDeath() {
		source.loop = false;
		source.PlayOneShot(death);
		source.loop = true;
	}

	public void playHurt() {
		source.loop = false;
		source.PlayOneShot(hurt);
		source.loop = true;
	}
}
