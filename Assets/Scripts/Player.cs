using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int fallDeathHeight;
    public GameObject ground;
    private GameObject player;
    private int health; 

	void Start () {
        player = gameObject;
        health = 3; 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        playerDeath();
	}

    void playerDeath() {
        if (player.transform.localPosition.y < ground.transform.localPosition.y + fallDeathHeight)
        {
            GameObject go = GameObject.Find("WorldGenerator");
            var worldGeneratorScript = (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
            worldGeneratorScript.RegenerateCurrentLevel();

            player.transform.localPosition = Vector3.zero;

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
