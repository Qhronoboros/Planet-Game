using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreadshipController : MonoBehaviour
{
    // public GameObject planetObj;
    public GameObject projectilePrefab;

    public float movementSpeed = 0f;
    // public float maxMovementSpeed = 1.0f;
    // public float accMovementSpeed = 1.0f;
    // public float decMovementSpeed = 1.0f;
    Vector2 direction;
    public float shootDelay = 0.2f;

    static PlayerController.MovementOptions currentMovement = PlayerController.MovementOptions.Default;

    float timeLastProjectile = 0;

    static bool holdShoot = false;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();
        
        if (joystickMovement.x < 0)
        {
            Debug.Log("holdLeft True");
            currentMovement = PlayerController.MovementOptions.Left;
            

        }
        else if (joystickMovement.x > 0)
        {
            Debug.Log("holdRight True");
            currentMovement = PlayerController.MovementOptions.Right;

        }
        // else if (joystickMovement.y > 0)
        // {
        //     Debug.Log("holdUp True");
        //     currentMovement = PlayerController.MovementOptions.Up;

        // }else if (joystickMovement.y < 0)
        // {
        //     Debug.Log("holdDown True");
        //     currentMovement = PlayerController.MovementOptions.Down;
        // }
        else
        {
            currentMovement = PlayerController.MovementOptions.Default;
            Debug.Log("neutral");
        }
        direction = joystickMovement;
        print(direction);
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
            laser.GetComponent<ProjectileController>().owner = this.tag;

            timeLastProjectile = Time.time;
        }

        if (currentMovement == PlayerController.MovementOptions.Left )
        {
            transform.Translate(direction * movementSpeed * Time.deltaTime);
        }
        else if (currentMovement == PlayerController.MovementOptions.Right)
        {
            transform.Translate(direction * movementSpeed * Time.deltaTime);
        }
        else
        {
            movementSpeed = 0;
        }
        // GetComponent<Rigidbody2D>().AddForce(new Vector2(movementSpeed, 0), ForceMode2D.Force);
        // transform.Translate(direction * movementSpeed * Time.deltaTime);
    }
}
