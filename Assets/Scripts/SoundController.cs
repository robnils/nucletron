using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
	
	public AudioSource source;
	public AudioClip backgroundMusic;
	public AudioClip[] hurt;
	public AudioClip[] death;
	public AudioClip[] fallDeath;
    public AudioClip levelComplete;

    // Use this for initialization
    void Awake () {
		source = GetComponent<AudioSource> ();
		playBackgroundMusic ();
	}

	public void playBackgroundMusic() {
		source.loop = true;
		source.PlayOneShot(backgroundMusic);
	}

	private AudioClip choseRandom(AudioClip[] lst) {
		return lst[Random.Range (0, lst.Length)];
	}

	public void playFallDeath() {
		source.loop = false;
		source.PlayOneShot(choseRandom(fallDeath));
		source.loop = true;
	}

    public void playLevelComplete() {
        source.loop = false;
        source.PlayOneShot(levelComplete);
        source.loop = true;
    }

    public void playDeath() {
		source.loop = false;
		source.PlayOneShot(choseRandom(death));
		source.loop = true;
	}

	public void playHurt() {
		source.loop = false;
		source.PlayOneShot(choseRandom(hurt));
		source.loop = true;
	}
}
