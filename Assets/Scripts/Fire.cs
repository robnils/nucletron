using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public AudioSource hit; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("FIIIIIIRE");
		if (other.gameObject.tag == "Player") {
			
			other.gameObject.transform.localPosition = Vector3.zero;

		}
	}
}
