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
    public int jumpCounter = 0;

    static MovementOptions currentMovement = MovementOptions.Default;
    public static float distance = 0;
    static bool isGrounded = false;

    float timeLastProjectile = 0;

    //static bool holdFly = false;
    static bool holdShoot = false;

    // Enums for the 3 movements options
    public enum MovementOptions
    {
        Default,
        Left,
        Right
    }

    // Reset Static Variables
    private void Awake()
    {
        currentMovement = MovementOptions.Default;
        distance = 0;
        isGrounded = false;
        //holdFly = false;
        holdShoot = false;
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
        if (value.started && jumpCounter < 3)
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
            jumpCounter += 1;
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        float joystickMovement = value.ReadValue<Vector2>().x;

        if (joystickMovement < 0)
        {
            Debug.Log("holdLeft True");
            currentMovement = MovementOptions.Left;

        }
        else if (joystickMovement > 0)
        {
            Debug.Log("holdRight True");
            currentMovement = MovementOptions.Right;
        }
        else
        {
            currentMovement = MovementOptions.Default;
            Debug.Log("neutral");
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            isGrounded = true;
            jumpCounter = 0;
        }
        else if (collision.gameObject.tag == "Asteroid")
        {
        int temp_life = GameManager.Instance.get_life();
        temp_life-=1;
        GameManager.Instance.set_life(temp_life);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            isGrounded = false;
        }
    }

    void Update()
    {
        // Keeps the player's rotation consistent
        transform.up = -(planet.transform.position - transform.position);

        // Shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<ProjectileController>().planet = planet;

            timeLastProjectile = Time.time;
        }

        // Distance between planet and player
        distance = Vector2.Distance(transform.position, planet.transform.position);

        //// Makes the player fly, hold spacebar to fly higher
        //if (holdFly && distance < 17.5f)
        //{
        //    GetComponent<Rigidbody2D>().AddForce(
        //        Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * flySpeed / Mathf.Max((distance - 7.0f) / 2.0f, 1.0f) * Time.deltaTime,
        //        ForceMode2D.Impulse);
        //}

        if (currentMovement == MovementOptions.Left && movementSpeed > -maxMovementSpeed)
        {
            movementSpeed -= accMovementSpeed * Time.deltaTime;
        }
        else if (currentMovement == MovementOptions.Right && movementSpeed < maxMovementSpeed)
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