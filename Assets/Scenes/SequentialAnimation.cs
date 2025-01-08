using UnityEngine;
using System.Collections;

public class SequentialAnimation : MonoBehaviour
{
    public Animator firstAnimator; // Animator for "end screen"
    public Animator secondAnimator; // Animator for "Egg man END"
    public GameObject firstObject; // GameObject for "end screen"
    public GameObject secondObject; // GameObject for "Egg man END"

    void Start()
    {
        secondObject.SetActive(false); // Ensure the second object is off initially
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        // Play the first animation
        firstAnimator.Play("end screen");

        // Wait for the first animation to finish
        yield return new WaitForSeconds(firstAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Deactivate the first object
        firstObject.SetActive(false);

        // Activate the second object
        secondObject.SetActive(true);

        // Play the second animation
        secondAnimator.Play("Egg man END");
    }
}