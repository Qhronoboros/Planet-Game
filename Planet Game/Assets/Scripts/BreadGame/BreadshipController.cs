using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreadshipController : MonoBehaviour
{
    // public GameObject planetObj;
    public GameObject projectilePrefab;
    public Transform main_camera;

    public float movementSpeed = 5.5f;
    public float x_clamp = 9;
    public float y_clamp = 4;

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
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, main_camera.position.x - x_clamp, main_camera.position.x +x_clamp), Mathf.Clamp(transform.position.y, main_camera.position.y - y_clamp, main_camera.position.y + y_clamp), transform.position.z);
    }
}
