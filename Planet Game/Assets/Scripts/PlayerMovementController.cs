using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject planet;
    public float movementSpeed = 0f;
    public float maxMovementSpeed = 10.0f;
    public float accMovementSpeed = 10.0f;
    public float decMovementSpeed = 10.0f;
    public float jumpHeight = 0.05f;

    static MovementOptions currentMovement = MovementOptions.Default;
    static float distance = 0;
    static bool grounded = false;

    // Enums for the 3 movements options
    public enum MovementOptions
    {
        Default,
        Left,
        Right
    }

    void Update()
    {
        // Distance between planet and player
        distance = Vector3.Distance(transform.position, planet.transform.position);

        // grounded is true if player is grounded, false if not
        if (distance <= 6.25f && !grounded) { grounded = true; }
        else if (distance > 6.25f && grounded) { grounded = false; }

        // Movement, A move accelerate left, D accelerate right
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentMovement = MovementOptions.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentMovement = MovementOptions.Right;
        }

        if (currentMovement == MovementOptions.Left && Input.GetKey(KeyCode.A) && movementSpeed > -maxMovementSpeed)
        {
            movementSpeed -= accMovementSpeed * Time.deltaTime;
        }
        else if (currentMovement == MovementOptions.Right && Input.GetKey(KeyCode.D) && movementSpeed < maxMovementSpeed)
        {
            movementSpeed += accMovementSpeed * Time.deltaTime;
        }
        else
        {
            if (movementSpeed > 0)
            {
                if (grounded)
                {
                    movementSpeed -= decMovementSpeed * 4 * Time.deltaTime;
                }
                else
                {
                    movementSpeed -= decMovementSpeed * Time.deltaTime;
                }
            }
            else if (movementSpeed < 0)
            {
                if (grounded)
                {
                    movementSpeed += decMovementSpeed * 4 * Time.deltaTime;
                }
                else
                {
                    movementSpeed += decMovementSpeed * Time.deltaTime;
                }
            }
        }

        // Move the player (Uses the planet to move around), the further the player is from the planet, the slower the player moves around
        transform.RotateAround(planet.transform.position, Vector3.back, movementSpeed / (distance * 0.2f) * Time.deltaTime);

        // Makes the player fly, hold spacebar to fly higher
        if (Input.GetKey(KeyCode.Space) && distance < 17.5f)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * jumpHeight / Mathf.Max((distance - 7.0f) / 2.0f, 1.0f) * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
