using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int fallDeathHeight;
    private GameObject player;

	private SoundController soundController;
	private Vector3 startingPosition; // FIXME refactor
	private int fallingHeight;

    private int health; 
    private const int maxHealth = 3;
	private bool alive;

	void Start () {
        player = gameObject;
        health = maxHealth; 
        updateHealthText();

		soundController = new SoundController ();

		GameObject go = GameObject.Find("MusicHandler");
		soundController = (SoundController)go.GetComponent(typeof(SoundController));
		alive = true;
		startingPosition = new Vector3 (0, 0, 0); 
		fallingHeight = (int)(startingPosition.y - 10.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        playerDeath();
	}

    public void resetPlayer() {
        updateHealth(maxHealth);
		movePlayerToStart();
		alive = true;
    }

    private void movePlayerToStart() {
        player.transform.localPosition = Vector3.zero;
    }

    private void updateHealth(int health) {
        this.health = health;
        updateHealthText();
    }
    private void updateHealthText() {
        GameObject objectText = GameObject.Find("Health");
        var txt = objectText.GetComponent<Text>();
        txt.text = "Health : " + health;
    }

    private void resetWorld() {
        GameObject go = GameObject.Find("WorldGenerator");
        var worldGeneratorScript = (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
        worldGeneratorScript.RegenerateCurrentLevel();
    }

	private bool isFalling(Vector3 pos) {
		return pos.y < fallingHeight;
	}

	private bool fallDepthDeath(Vector3 pos) {
		return pos.y < fallingHeight + fallDeathHeight;
	}

    void playerDeath() {
        if (health <= 0) {
			Debug.Log ("Health depleted, dead");
            resetPlayer();
        }

		if (isFalling (player.transform.localPosition)) {
			Debug.Log ("Falling");
			if (alive) {
				Debug.Log ("playing fall sound effect");
				soundController.playFallDeath();
				alive = false;
			}


			if (fallDepthDeath(player.transform.localPosition)) {
				Debug.Log ("Fell to gruesome death");
				resetPlayer();
			}
		}
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Neutron_fire" || collision.gameObject.name == "WallOfFire") {
            updateHealth(health--);
        }
    }
}


/*
 * 
 * 


	void Start () {
         //   restartLocation = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (transform.localPosition.y < -fallDeathHeight)
        {
            transform.localPosition = restartLocation;
        }
	}
 * */
