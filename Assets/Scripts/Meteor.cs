using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour {

    public float attractionForce;
    private GameObject player;
    public Transform explosionPrefab;

    public int selfDestructHeight;
    private int selfDestructVariation;

    private float timeToLive;
    private float timSinceSpawn;

    Rigidbody body;

    void Awake() {
        body = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeToLive = Random.Range(5.0f, 15.0f);

        selfDestructVariation = (int)(selfDestructHeight * 0.1f);

        if (player == null) {
            throw new System.Exception("player not found, programming error");
        }
    }

    void FixedUpdate() {
        // body.AddForce(player.transform.localPosition * -attractionForce);

        int selfDestructOffsetRandom = selfDestructHeight + Random.Range(selfDestructHeight - selfDestructVariation, selfDestructHeight + selfDestructVariation);
        if (transform.localPosition.y < selfDestructOffsetRandom) {
            //DestroyMeteor(transform.localPosition, transform.localRotation);
			Destroy(gameObject);
        }

        timSinceSpawn += Time.deltaTime;
        if (timSinceSpawn >= timeToLive) {
            timSinceSpawn -= timeToLive;
            DestroyMeteor(transform.localPosition, transform.localRotation);
        }
        //body.AddForce(transform.localPosition * -attractionForce);
    }

    private void DestroyMeteor(Vector3 pos, Quaternion rot) {
        Instantiate(explosionPrefab, pos, rot);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Collided with player");
        }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        DestroyMeteor(pos, rot);
    }
}