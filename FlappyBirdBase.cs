using UnityEngine;

public class FlappyBirdBase : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // Speed at which the base moves to the left
    public float resetPositionX = -10f; // X position at which the base resets
    public float startPositionX = 10f; // X position to reset to

    private Vector3 startPosition;

    private void Start()
    {
        // Store the initial position of the base
        startPosition = transform.position;
    }

    private void Update()
    {
        // Move the base to the left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Check if the base has moved past the reset position
        if (transform.position.x <= resetPositionX)
        {
            // Reset the base to its starting position
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        // Reset the position of the base
        transform.position = new Vector3(startPositionX, startPosition.y, startPosition.z);
    }
}
