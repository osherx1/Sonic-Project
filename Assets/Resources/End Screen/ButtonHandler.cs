using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public GameObject button; // Assign the Text (Legacy) button here

    // Function triggered by the Animation Event
    public void ShowButton()
    {
        if (button != null)
        {
            button.SetActive(true); // Enable the button

        }
    }
}
