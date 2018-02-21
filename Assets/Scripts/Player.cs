using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int fallDeathHeight;
    private GameObject player;

	private SoundController soundController;
    private WorldGenerator worldGenerator;

    private Vector3 startingPosition; // FIXME refactor
	private int fallingHeight;

    private int health; 
    private const int maxHealth = 3;
	private bool alive;

	void Start () {
        player = gameObject;
        health = maxHealth; 
		UpdateHealthText ();

		soundController = GetSoundController();
        worldGenerator = GetWorldGenerator();

		alive = true;
		startingPosition = new Vector3 (0, 0, 0); 
		fallingHeight = (int)(startingPosition.y - 5.0f);
	}

	private SoundController GetSoundController() {
		var go = GameObject.Find("MusicHandler");
		return (SoundController)go.GetComponent(typeof(SoundController));
	}

    private WorldGenerator GetWorldGenerator() {
        GameObject go = GameObject.Find("WorldGenerator");
        return (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
    }

    // Update is called once per frame
    void FixedUpdate () {
        PlayerDeath();
	}

    public void ResetPlayer() {
        UpdateHealth(maxHealth);
		MovePlayerToStart();
		alive = true;
    }

    public void MovePlayerToStart() {
        player.transform.localPosition = Vector3.zero;
    }

    private void UpdateHealth(int health) {
        this.health = health;
        UpdateHealthText();
    }
    private void UpdateHealthText() {
        GameObject objectText = GameObject.Find("Health");
        var txt = objectText.GetComponent<Text>();
        txt.text = "Health : " + health;
    }

    public void ResetWorld() {
        worldGenerator.RegenerateCurrentLevel();
    }

	private bool IsFalling(Vector3 pos) {
		return pos.y < fallingHeight;
	}

	private bool FallDepthDeath(Vector3 pos) {
		return pos.y < fallingHeight + fallDeathHeight;
	}

    void PlayerDeath() {
        if (health <= 0) {
			Debug.Log ("Health depleted, dead");
            ResetPlayer();
        }

		if (IsFalling (player.transform.localPosition)) {
			Debug.Log ("Falling");
			if (alive) {
				Debug.Log ("playing fall sound effect");
				soundController.playFallDeath();
				alive = false;
			}


			if (FallDepthDeath(player.transform.localPosition)) {
				Debug.Log ("Fell to gruesome death");
				ResetPlayer();
			}
		}
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Neutron_fire" || collision.gameObject.name == "WallOfFire") {
            UpdateHealth(health--);
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
