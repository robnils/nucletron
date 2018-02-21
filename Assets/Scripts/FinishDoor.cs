using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour {

    private SoundController soundController;
    private WorldGenerator worldGenerator;
    private Player player;

    void Start () {
        soundController = GetSoundController();
        worldGenerator = GetWorldGenerator();
        player = GetPlayer();
    }

    private WorldGenerator GetWorldGenerator() {
        GameObject go = GameObject.Find("WorldGenerator");
        return (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
    }

    // Update is called once per frame
    void Update () {
		
	}

    private SoundController GetSoundController() {
        var go = GameObject.Find("MusicHandler");
        return (SoundController)go.GetComponent(typeof(SoundController));
    }

    private Player GetPlayer() {
        var go = GameObject.FindGameObjectWithTag("Player");
        return (Player)go.GetComponent(typeof(Player));
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("finish");
        if (other.gameObject.tag == "Player") {
            Debug.Log("Hit finish");
            player.MovePlayerToStart();
            soundController.playLevelComplete();
            worldGenerator.NextLevel();
            /*
            GameObject go = GameObject.Find("WorldGenerator");
            var worldGeneratorScript = (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
            worldGeneratorScript.NextLevel();
            */
        }
    }

    /*
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Player") {
            Debug.Log("Collided");
        }

        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collidedtag");
        }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        
    }
    */
}
