using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreadshipController : MonoBehaviour
{
    public GameObject planetObj;
    public GameObject projectilePrefab;

    public float movementSpeed = 0f;
    public float maxMovementSpeed = 10.0f;
    public float accMovementSpeed = 10.0f;
    public float decMovementSpeed = 10.0f;
    public float shootDelay = 0.2f;

    static PlayerController.MovementOptions currentMovement = PlayerController.MovementOptions.Default;

    float timeLastProjectile = 0;

    static bool holdShoot = false;

    public void OnMovement(InputAction.CallbackContext value)
    {
        float joystickMovement = value.ReadValue<Vector2>().x;

        if (joystickMovement < 0)
        {
            Debug.Log("holdLeft True");
            currentMovement = PlayerController.MovementOptions.Left;

        }
        else if (joystickMovement > 0)
        {
            Debug.Log("holdRight True");
            currentMovement = PlayerController.MovementOptions.Right;
        }
        else
        {
            currentMovement = PlayerController.MovementOptions.Default;
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

    void Start()
    {
        
    }

    void Update()
    {
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.GetComponent<ProjectileController>().owner = planetObj.tag;

            timeLastProjectile = Time.time;
        }

        if (currentMovement == PlayerController.MovementOptions.Left && movementSpeed > -maxMovementSpeed)
        {
            if (movementSpeed > 0.1f)
            {
                movementSpeed -= decMovementSpeed * 2 * Time.deltaTime;
            }
            else
            {
                movementSpeed -= accMovementSpeed * Time.deltaTime;
            }
        }
        else if (currentMovement == PlayerController.MovementOptions.Right && movementSpeed < maxMovementSpeed)
        {
            if (movementSpeed < -0.1f)
            {
                movementSpeed += decMovementSpeed * 2 * Time.deltaTime;
            }
            else
            {
                movementSpeed += accMovementSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (movementSpeed > 0.1f)
            {
                movementSpeed -= decMovementSpeed * 2 * Time.deltaTime;
            }
            else if (movementSpeed < -0.1f)
            {
                movementSpeed += decMovementSpeed * 2 * Time.deltaTime;
            }
            else
            {
                movementSpeed = 0;
            }
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(movementSpeed, 0), ForceMode2D.Force);
        //transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
    }
}
