using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public int fallDeathHeight;
    public GameObject ground;
    private GameObject player;

    private int health; 
    private const int maxHealth = 3;

	void Start () {
        player = gameObject;
        health = maxHealth; 
        updateHealthText();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        playerDeath();
	}

    public void resetPlayer() {
        updateHealth(maxHealth);
    }

    private void movePlayerToStar() {
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

    void playerDeath() {
        if (health <= 0) {
            resetPlayer();
        }

        if (player.transform.localPosition.y < ground.transform.localPosition.y + fallDeathHeight) {
            resetPlayer();
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
