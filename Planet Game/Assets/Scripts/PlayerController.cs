using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No PlayerController instance");

            return _instance;
        }
    }

    public GameObject mainPlanetObj;
    public GameObject projectilePrefab;
    public float movementSpeed = 0f;
    public float maxMovementSpeed = 10.0f;
    public float accMovementSpeed = 10.0f;
    public float decMovementSpeed = 10.0f;
    public float jumpHeight = 5.0f;
    public float doubleJumpHeight = 1.0f;
    public float flySpeed = 5.0f;
    public float shootDelay = 0.2f;
    private int jumpCounter = 0;
    private Gravity gravity;

    public float horizontalMovement = 0.0f;
    public float walkspeed = 10.0f;
    public float maxVelocityChange = 1.0f;
    public float multiplyDownward = 2;
    public Vector2 lastJoystickVector;

    static MovementOptions currentMovement = MovementOptions.Default;
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
        isGrounded = false;
        //holdFly = false;
        holdShoot = false;

        // If planet not assigned, check for closest planet
        if (!mainPlanetObj)
        {
            GameObject closestPlanet = null;
            float smallestDistance = Mathf.Infinity;

            foreach (GameObject planet in GameManager.Instance.planets)
            {
                float distance = planet.GetComponent<PlanetScript>().calcDistance(gameObject);
                if (distance < smallestDistance)
                {
                    closestPlanet = planet;
                    smallestDistance = distance;
                }
            }

            mainPlanetObj = closestPlanet;
        }

        // Give gravity script to self
        gravity = gameObject.AddComponent<Gravity>();
        gravity.assignPlanet(mainPlanetObj);
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

    public void ChangePlanet(GameObject planet)
    {
        mainPlanetObj = planet;
        gravity.assignPlanet(planet);
        GameManager.Instance.setPlayerPlanet(planet);
        GameManager.Instance.cameraController.UpdateCameraSettings(planet);
        Debug.Log("Done changing planet");
    }

    public void AddPlanet(GameObject planet)
    {
        gravity.planetsOrbiting.Add(planet);
    }

    public void RemovePlanet(GameObject planet)
    {
        gravity.planetsOrbiting.Remove(planet);
        
        if (gravity.planetsOrbiting.Count == 0)
        {
            // Kill player
            GameManager.Instance.set_life(0);
        }
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started && jumpCounter < 3)
        {
            if (isGrounded)
            {
                Debug.Log("Jump");
                GetComponent<Rigidbody2D>().AddForce(-(mainPlanetObj.transform.position - transform.position) * jumpHeight, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Double Jump");
                GetComponent<Rigidbody2D>().AddForce(-(mainPlanetObj.transform.position - transform.position) * doubleJumpHeight, ForceMode2D.Impulse);
            }
            jumpCounter += 1;
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        float joystickMovement = value.ReadValue<Vector2>().x;

        if (joystickMovement < 0)
        {
            horizontalMovement = -1;
            //currentMovement = MovementOptions.Left;

        }
        else if (joystickMovement > 0)
        {
            horizontalMovement = 1;
            //currentMovement = MovementOptions.Right;
        }
        else
        {
            horizontalMovement = 0;
            //currentMovement = MovementOptions.Default;
        }
    }

    public void OnJumping(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();
        if (value.started)
        {
            lastJoystickVector = new Vector2(0, 0);
            Debug.Log("start");
        }

        if (Vector2.Distance(joystickMovement, new Vector2(0, 0)) < 0.5f && value.canceled)
        {
            GetComponent<Rigidbody2D>().AddForce(-(GameManager.Instance.cameraController.transform.TransformDirection(lastJoystickVector)) * jumpHeight, ForceMode2D.Impulse);
            Debug.Log(lastJoystickVector);
            Debug.Log("cancel");
        }

        if (Vector2.Distance(joystickMovement, new Vector2(0, 0)) > 0.5f)
        {
            lastJoystickVector = joystickMovement;
        }
    }

    public void OnShoot(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            holdShoot = true;
        }
        else if (value.canceled)
        {
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

    private void FixedUpdate()
    {
        float distance = mainPlanetObj.GetComponent<PlanetScript>().calcDistance(gameObject);
        float randomCalc = (Mathf.Sqrt(Mathf.Pow(distance, 2.0f) - Mathf.Pow(Mathf.Abs(horizontalMovement), 2.0f)) - distance)
            * multiplyDownward * (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y)) / walkspeed * 2;

        Debug.Log(randomCalc);

        //Debug.Log(distance.ToString() + " " + Mathf.Sqrt(Mathf.Pow(distance, 2.0f) - Mathf.Pow(Mathf.Abs(horizontalMovement), 2.0f)).ToString() + " " + randomCalc.ToString());

        // Calculate how fast we should be moving
        var targetVelocity = new Vector2(horizontalMovement, randomCalc);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= walkspeed;

        //Debug.Log(targetVelocity);

        // Apply a force that attempts to reach our target velocity
        var velocity = GetComponent<Rigidbody2D>().velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        //velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
        GetComponent<Rigidbody2D>().AddForce(velocityChange, ForceMode2D.Force);

        // Keeps the player's rotation consistent
        transform.up = -(mainPlanetObj.transform.position - transform.position);

        // Shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<ProjectileController>().planetObj = mainPlanetObj;

            timeLastProjectile = Time.time;
        }

        //// Makes the player fly, hold spacebar to fly higher
        //if (holdFly && distance < 17.5f)
        //{
        //    GetComponent<Rigidbody2D>().AddForce(
        //        Vector2.MoveTowards(transform.position, planet.transform.position, Time.deltaTime) * flySpeed / Mathf.Max((distance - 7.0f) / 2.0f, 1.0f) * Time.deltaTime,
        //        ForceMode2D.Impulse);
        //}

        //if (currentMovement == MovementOptions.Left && movementSpeed > -maxMovementSpeed)
        //{
        //    movementSpeed -= accMovementSpeed * Time.deltaTime;
        //}
        //else if (currentMovement == MovementOptions.Right && movementSpeed < maxMovementSpeed)
        //{
        //    movementSpeed += accMovementSpeed * Time.deltaTime;
        //}
        //else
        //{
        //    if (movementSpeed > 0.1)
        //    {
        //        if (isGrounded)
        //        {
        //            movementSpeed -= decMovementSpeed * 4 * Time.deltaTime;
        //        }
        //        else
        //        {
        //            movementSpeed -= decMovementSpeed * Time.deltaTime;
        //        }
        //    }
        //    else if (movementSpeed < -0.1f)
        //    {
        //        if (isGrounded)
        //        {
        //            movementSpeed += decMovementSpeed * 4 * Time.deltaTime;
        //        }
        //        else
        //        {
        //            movementSpeed += decMovementSpeed * Time.deltaTime;
        //        }
        //    }
        //    else
        //    {
        //        movementSpeed = 0;
        //    }
        //}

        //// Move the player (Uses the planet to move around), the further the player is from the planet, the slower the player moves around
        //transform.RotateAround(mainPlanetObj.transform.position, Vector3.back, movementSpeed / (mainPlanetObj.GetComponent<PlanetScript>().calcDistance(gameObject) * 0.2f) * Time.deltaTime);
    }
}