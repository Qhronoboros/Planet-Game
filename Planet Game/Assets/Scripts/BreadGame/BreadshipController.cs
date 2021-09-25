using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreadshipController : MonoBehaviour
{
    // public GameObject planetObj;
    public GameObject projectilePrefab;

    public float movementSpeed = 5.5f;
    // public float maxMovementSpeed = 1.0f;
    // public float accMovementSpeed = 1.0f;
    // public float decMovementSpeed = 1.0f;
    Vector2 direction;
    public float shootDelay = 0.2f;
    public int laser_layer = 4;
    static PlayerController.MovementOptions currentMovement = PlayerController.MovementOptions.Default;

    float timeLastProjectile = 0;

    static bool holdShoot = false;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();
        direction = joystickMovement;

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
        //shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.GetChild(0).rotation);
            laser.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
            laser.GetComponent<SpriteRenderer>().sortingOrder = laser_layer;
            laser.GetComponent<ProjectileController>().owner = this.tag;
            laser.GetComponent<ProjectileController>().aimDirection = Vector2.up;
            timeLastProjectile = Time.time;
        }
        //movement
        transform.Translate((direction+ new Vector2(0.5f,0f)) * movementSpeed * Time.deltaTime);
    }
}
