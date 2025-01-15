using PolyverseSkiesAsset;
using UnityEngine;


public class TimeOfDayCycler : MonoBehaviour
{
    [SerializeField] private float cycleDuration = 30f; // Total duration of a full cycle (in seconds)
    private float cycleTimer; // Timer to track progress of the current cycle
    private bool increasing = true; // Tracks whether we're increasing or decreasing
    [SerializeField] private PolyverseSkies TimeOfDay; // Reference to the TimeOfDay component

    void Start()
    {
        // Get the TimeOfDay component from the Polyverse Skies system
        TimeOfDay = GetComponent<PolyverseSkies>();

        if (TimeOfDay == null)
        {
            Debug.LogError("TimeOfDay component not found! Please attach this script to an object with the Polyverse Skies TimeOfDay component.");
            enabled = false; // Disable this script to avoid errors
            return;
        }

        cycleTimer = 0f;
    }

    void FixedUpdate()
    {
        if (TimeOfDay == null) return;

        // Increment or decrement the timer based on the direction
        float delta = Time.deltaTime / cycleDuration;
        cycleTimer += increasing ? delta : -delta;

        // Clamp the cycleTimer to ensure it's between 0 and 1
        cycleTimer = Mathf.Clamp01(cycleTimer);

        // Update the TimeOfDay property
        TimeOfDay.timeOfDay = Mathf.SmoothStep(0f, 1f, cycleTimer);

        // Reverse direction when the timer reaches its bounds
        if (cycleTimer >= 1f)
        {
            increasing = false;
        }
        else if (cycleTimer <= 0f)
        {
            increasing = true;
        }
    }
}
