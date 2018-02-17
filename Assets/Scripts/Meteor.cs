using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour {

    Rigidbody body; 
    void Awake() {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        body.AddForce(transform.localPosition * -10);
    }
}