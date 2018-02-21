using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private SoundController soundController;
    private GameObject boundary;
    private Player player;


    // Use this for initialization
    void Awake () {
		soundController = getSoundController ();
        boundary = GameObject.Find("Boundary");
        player = GetPlayer(); // TODO move all instances to gameController
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Player GetPlayer() {
        var go = GameObject.FindGameObjectWithTag("Player");
        return (Player)go.GetComponent(typeof(Player));
    }

    private SoundController getSoundController() {
		var go = GameObject.Find("MusicHandler");
		return (SoundController)go.GetComponent(typeof(SoundController));
	}
    
	void OnTriggerEnter(Collider other) {
		Debug.Log("FIIIIIIRE");
		if (other.gameObject.tag == "Player") {
            Debug.Log("it hurts!");


            player.TakeDamage();

            // FIXME doesn't work yet
            float explosionStrength = 10.0f;
            var body = other.gameObject.GetComponent<Rigidbody>();
            Vector3 forceVec = -body.velocity.normalized * explosionStrength;
            body.AddForce(forceVec, ForceMode.Acceleration);
        }
	}
    



}
