using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {

    public float attractionForce;
    private GameObject player;
    public Transform explosionPrefab;

    Rigidbody body;

    void Awake() {
        body = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) {
            throw new System.Exception("player not found, programming error");
        }
    }

    void FixedUpdate() {
        // body.AddForce(player.transform.localPosition * -attractionForce);
        /*
        if (transform.localPosition.y < 10) {
            Destroy(this);
        }
        */
        body.AddForce(transform.localPosition * -attractionForce);
    }

    void OnCollisionEnter(Collision collision) {
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        Instantiate(explosionPrefab, pos, rot);
        Destroy(gameObject);
    }
}