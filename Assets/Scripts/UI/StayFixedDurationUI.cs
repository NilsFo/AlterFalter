using UnityEngine;
using UnityEngine.UI;

public class StayFixedDurationUI : MonoBehaviour
{
    public Better_Worm_Movement wormMovement;
    public Text durationText;
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
            durationText.transform.position = Camera.main.WorldToScreenPoint(wormMovement.transform.position + offset);
        }
        else
        {
            durationText.text = "";
        }
    }
}
