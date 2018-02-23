using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private SoundController soundController;
    private GameObject boundary;
    private Player player;

    private float timeToLive;
    private float timeAlive;

    // Use this for initialization
    void Awake () {
		soundController = getSoundController ();
        boundary = GameObject.Find("Boundary");
        player = GetPlayer(); // TODO move all instances to gameController
        //timeToLive = Random.Range(5.0f, 5.0f);
		timeToLive = 3.0f;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timeAlive += Time.deltaTime;
        if (timeAlive >= timeToLive) {
            Destroy(transform.parent.gameObject);
        }
    }

    public void SetTimeToLive(float time) {
        timeToLive = time;
    }

	public void AddTimeToLive(float time) {
		timeToLive += time;
	}

	public void AddTimeToLive(int time) {
		AddTimeToLive((float)time);
	}

    private Player GetPlayer() {
        var go = GameObject.FindGameObjectWithTag("Player");
        return (Player)go.GetComponent(typeof(Player));
    }

    private SoundController getSoundController() {
		var go = GameObject.Find("MusicHandler");
		return (SoundController)go.GetComponent(typeof(SoundController));
	}

	void TakeFireDamage(Collider other) {
		player.TakeDamage();

		// FIXME doesn't work yet
		float explosionStrength = 10.0f;
		var body = other.gameObject.GetComponent<Rigidbody>();
		Vector3 forceVec = -body.velocity.normalized * explosionStrength;
		body.AddForce(forceVec, ForceMode.Acceleration);
	}
    
	void OnTriggerEnter(Collider other) {
		Debug.Log("FIIIIIIRE");
		if (other.gameObject.tag == "Player") {
			TakeFireDamage(other);
		}
	}

	void OnTriggerStay(Collider other) {
		TakeFireDamage(other);
	}
    



}
