using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private SoundController soundController; 

	// Use this for initialization
	void Awake () {
		soundController = getSoundController ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private SoundController getSoundController() {
		var go = GameObject.Find("MusicHandler");
		return (SoundController)go.GetComponent(typeof(SoundController));
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("FIIIIIIRE");
		soundController.playHurt ();
		if (other.gameObject.tag == "Player") {
			
		}
	}
}
