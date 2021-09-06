using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject planet;
    public GameObject projectilePrefab;

    public float movementSpeed = 0f;
    public float maxMovementSpeed = 10.0f;
    public float accMovementSpeed = 10.0f;
    public float decMovementSpeed = 10.0f;
    public float jumpHeight = 5.0f;
    public float doubleJumpHeight = 1.0f;
    public float flySpeed = 5.0f;
    public float shootDelay = 0.2f;

    static MovementOptions currentMovement = MovementOptions.Default;
    public static float distance = 0;
    static bool isGrounded = false;

    float timeLastProjectile = 0;

    static bool holdFly = false;
    static bool holdLeft = false;
    static bool holdRight = false;
    static bool holdShoot = false;

    // Enums for the 3 movements options
    public enum MovementOptions
    {
        Default,
        Left,
        Right
    }

    //public void OnFly(InputAction.CallbackContext value)
    //{
    //    if (value.started)
    //    {
    //        Debug.Log("holdFly True");
    //        holdFly = true;
    //    }
    //    else if (value.canceled)
    //    {
    //        Debug.Log("holdFly False");
    //        holdFly = false;
    //    }
    //}

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (isGrounded)
            {
                Debug.Log("Jump");
                GetComponent<Rigidbody2D>().AddForce(
                    Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * jumpHeight,
                    ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Double Jump");
                GetComponent<Rigidbody2D>().AddForce(
                    Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * doubleJumpHeight,
                    ForceMode2D.Impulse);
            }
        }
    }

    public void OnLeft(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("holdLeft True");
            currentMovement = MovementOptions.Left;
            holdLeft = true;
        }
        else if (value.canceled)
        {
            Debug.Log("holdLeft False");
            holdLeft = false;
        }
    }

    public void OnRight(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("holdRight True");
            currentMovement = MovementOptions.Right;
            holdRight = true;
        }
        else if (value.canceled)
        {
            Debug.Log("holdRight False");
            holdRight = false;
        }
    }

    public void OnShoot(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("holdShoot True");
            holdShoot = true;
        }
        else if (value.canceled)
        {
            Debug.Log("holdShoot False");
            holdShoot = false;
        }
    }

    void Update()
    {
        // Keeps the player's rotation consistent
        transform.up = planet.transform.position - transform.position;

        // Shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<ProjectileController>().planet = planet;

            timeLastProjectile = Time.time;
        }

        // Distance between planet and player
        distance = Vector2.Distance(transform.position, planet.transform.position);

        // Makes the player fly, hold spacebar to fly higher
        if (holdFly && distance < 17.5f)
        {
            GetComponent<Rigidbody2D>().AddForce(
                Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * flySpeed / Mathf.Max((distance - 7.0f) / 2.0f, 1.0f) * Time.deltaTime,
                ForceMode2D.Impulse);
        }

        // grounded is true if player is grounded, false if not
        if (distance <= 6.25f && !isGrounded) { isGrounded = true; }
        else if (distance > 6.25f && isGrounded) { isGrounded = false; }

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
                if (isGrounded)
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
                if (isGrounded)
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