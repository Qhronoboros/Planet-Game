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
    public int jumpLimit = 3;
    public Gravity gravity;
    public float spinRotation = 20.0f;

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
                float distance = planet.GetComponent<PlanetScript>().calcDistance(gameObject, false);
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
        Debug.Log("Changed mainPlanet");
    }

    public void AddPlanet(GameObject planet)
    {

        gravity.planetsOrbiting.Add(planet);
    }

    public void RemovePlanet(GameObject planet)
    {
        gravity.planetsOrbiting.Remove(planet);

        if (planet == mainPlanetObj && gravity.planetsOrbiting.Count != 0)
        {
            ChangePlanet(gravity.planetsOrbiting[gravity.planetsOrbiting.Count - 1]);
        }

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
        // joystick start moving
        if (value.started)
        {
            lastJoystickVector = new Vector2(0, 0);
        }

        // joystick release
        if (Vector2.Distance(joystickMovement, new Vector2(0, 0)) < 0.5f && value.canceled)
        {
            GetComponent<Rigidbody2D>().AddForce(-(GameManager.Instance.cameraController.transform.TransformDirection(lastJoystickVector)) * jumpHeight * 15, ForceMode2D.Impulse);
            jumpCounter += 1;
        }

        // joysting moving
        if (Vector2.Distance(joystickMovement, new Vector2(0, 0)) > 0.5f && jumpCounter < jumpLimit)
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
        // Calculate vertical velocity
        float distance = mainPlanetObj.GetComponent<PlanetScript>().calcDistance(gameObject, false);
        float randomCalc = (Mathf.Sqrt(Mathf.Pow(distance, 2.0f) - Mathf.Pow(Mathf.Abs(horizontalMovement), 2.0f)) - distance)
            * multiplyDownward * (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y)) * (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y)) / 20;

        // Decrease vertical velocity when grounded
        if (isGrounded)
        {
            randomCalc /= maxVelocityChange;
        }

        // Calculate speed
        var targetVelocity = new Vector2(horizontalMovement, randomCalc);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= walkspeed;

        // Apply a force that attempts to reach our target velocity
        var velocity = GetComponent<Rigidbody2D>().velocity;
        var velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
        GetComponent<Rigidbody2D>().AddForce(velocityChange, ForceMode2D.Force);

        // Keeps the player's rotation consistent
        Vector3 relativePos = mainPlanetObj.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, -relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, spinRotation * 2 / mainPlanetObj.GetComponent<PlanetScript>().calcDistance(gameObject, true) * Time.deltaTime);

        // Shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position + transform.up, transform.rotation);
            laser.GetComponent<ProjectileController>().owner = gameObject;

            timeLastProjectile = Time.time;
        }
    }
}