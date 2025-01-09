using UnityEngine;

public class SnapToPixel : MonoBehaviour {
    new Rigidbody rigidbody;
    public void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }
}