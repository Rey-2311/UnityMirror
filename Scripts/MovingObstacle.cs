using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float distance = 5f;         // Distance the obstacle moves before reversing direction
    public float speed = 3f;            // Speed at which the obstacle moves

    public enum Direction { Right, Left, Up, Down }  // Enum for selecting direction
    public Direction moveDirection = Direction.Right;

    private Vector3 startingPosition;
    private Vector3 directionVector;
    private bool movingForward = true;

    void Start()
    {
        startingPosition = transform.position;
        SetDirectionVector();
    }

    void Update()
    {
        // Calculate movement direction and position
        float movementFactor = movingForward ? 1f : -1f;
        transform.position += directionVector * movementFactor * speed * Time.deltaTime;

        // Check if the obstacle has moved the full distance
        if (Vector3.Distance(transform.position, startingPosition) >= distance)
        {
            // Reverse the movement direction
            movingForward = !movingForward;
            // Reset the starting position for the next leg of the journey
            startingPosition = transform.position;
        }
    }

    // Set the movement direction based on user selection in the editor
    void SetDirectionVector()
    {
        switch (moveDirection)
        {
            case Direction.Right:
                directionVector = Vector3.right;
                break;
            case Direction.Left:
                directionVector = Vector3.left;
                break;
            case Direction.Up:
                directionVector = Vector3.up;
                break;
            case Direction.Down:
                directionVector = Vector3.down;
                break;
        }
    }
}
