using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public GameObject nextObject; // Assign the "Egg man END" GameObject

    public void OnEndScreenComplete()
    {
        gameObject.SetActive(false); // Disable this object
        if (nextObject != null)
        {
            nextObject.SetActive(true); // Enable the next object
        }
    }
}
