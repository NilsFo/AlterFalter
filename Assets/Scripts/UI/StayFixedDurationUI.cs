using UnityEngine;
using TMPro;

public class StayFixedDurationUI : MonoBehaviour
{
    public Better_Worm_Movement wormMovement;
    public TextMeshProUGUI durationText;
    public Vector3 offset;

    void Start()
    {
        if (durationText != null)
        {
            durationText.color = Color.white;
        }
    }

    void Update()
    {
        Better_Worm_Movement wormMovement = FindObjectOfType<Better_Worm_Movement>();

        if (wormMovement != null)
        {
            float remainingDuration = wormMovement.GetRemainingStayFixedDuration();
            durationText.text = "Grip: " + remainingDuration.ToString("F1");
        }

        UpdateDurationText(); // Call this function in the Update method
    }

    void UpdateDurationText()
    {
        if (wormMovement == null || durationText == null)
        {
            return;
        }

        float remainingDuration = wormMovement.GetRemainingStayFixedDuration();

        if (remainingDuration > 0f)
        {
            durationText.text = "Grip: " + remainingDuration.ToString("F1");

            // Get the RectTransform component of the durationText object
            RectTransform durationTextRectTransform = durationText.GetComponent<RectTransform>();

            // Set the anchored position of the durationText in screen space
            durationTextRectTransform.position = Camera.main.WorldToScreenPoint(wormMovement.transform.position + offset);
        }
        else
        {
            durationText.text = "";
        }
    }
}
