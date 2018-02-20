using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Collidedtag");
            other.gameObject.transform.localPosition = Vector3.zero;

            GameObject go = GameObject.Find("WorldGenerator");
            var worldGeneratorScript = (WorldGenerator)go.GetComponent(typeof(WorldGenerator));
            worldGeneratorScript.NextLevel();

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
