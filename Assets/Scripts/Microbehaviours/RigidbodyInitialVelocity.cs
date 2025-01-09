using UnityEngine;

public class RigidbodyInitialVelocity : MonoBehaviour {
    public Vector3 intialVelocity;

    void Start() {
        GetComponent<Rigidbody>().linearVelocity = intialVelocity;
        Destroy(this);
    }
}
