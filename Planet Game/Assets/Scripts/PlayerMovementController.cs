using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    static bool holdFly = false;
    static bool holdLeft = false;
    static bool holdRight = false;

    // Enums for the 3 movements options
    public enum MovementOptions
    {
        Default,
        Left,
        Right
    }

    public void OnFly(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            holdFly = true;
        }
        else
        {
            holdFly = false;
        }
    }

    public void OnLeft(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            currentMovement = MovementOptions.Left;
            holdLeft = true;
        }
        else
        {
            holdLeft = false;
        }
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            currentMovement = MovementOptions.Right;
            holdRight = true;
        }
        else
        {
            holdRight = false;
        }
    }

    void Update()
    {
        // Distance between planet and player
        distance = Vector2.Distance(transform.position, planet.transform.position);

        // Makes the player fly, hold spacebar to fly higher
        if (holdFly && distance < 17.5f)
        {
            Debug.Log("fly");
            GetComponent<Rigidbody2D>().AddForce(Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * jumpHeight / Mathf.Max((distance - 7.0f) / 2.0f, 1.0f) * Time.deltaTime, ForceMode2D.Impulse);
        }

        // grounded is true if player is grounded, false if not
        if (distance <= 6.25f && !grounded) { grounded = true; }
        else if (distance > 6.25f && grounded) { grounded = false; }

        if (currentMovement == MovementOptions.Left && holdLeft && movementSpeed > -maxMovementSpeed)
        {
            movementSpeed -= accMovementSpeed * Time.deltaTime;
        }
        else if (currentMovement == MovementOptions.Right && holdRight && movementSpeed < maxMovementSpeed)
        {
            movementSpeed += accMovementSpeed * Time.deltaTime;
        }
        else
        {
            if (movementSpeed > 0.1)
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
            else if (movementSpeed < -0.1f)
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
            else
            {
                movementSpeed = 0;
            }
        }

        // Move the player (Uses the planet to move around), the further the player is from the planet, the slower the player moves around
        transform.RotateAround(planet.transform.position, Vector3.back, movementSpeed / (distance * 0.2f) * Time.deltaTime);
    }
}