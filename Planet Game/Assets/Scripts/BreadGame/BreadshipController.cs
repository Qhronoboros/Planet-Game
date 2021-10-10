using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BreadshipController : MonoBehaviour
{
    //jumps
    public GameObject jumpArrow;
    public Text jumpCounterText;
    private int jumpCounter = 0;
    public int jumpLimit = 3;
    public float jumpHeight = 1.0f;
    public float doubleJumpHeight = 1.0f;
    public Vector2 lastJoystickVector;
    //
    public GameObject projectilePrefab;
    public Transform main_camera;
    public AudioClip jumpAudio;
    public AudioClip hitAudio;
    public float movementSpeed = 5.5f;
    public float x_clamp = 9;
    public float y_clamp = 4;
    public Vector2 idle_movespeed = new Vector2(0.5f,0);

    Vector2 direction;
    public float shootDelay = 0.2f;
    public int laser_layer = 4;
    // static PlayerController.MovementOptions currentMovement = PlayerController.MovementOptions.Default;

    float timeLastProjectile = 0;

    static bool holdShoot = false;
    public bool invincibility = false;
    public float invincibilityTime = 1.0f;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();
        direction = joystickMovement;

    }
    public void OnHit()
    {
        if (!Game_Manager.playerDead && !Game_Manager.Instance.stageClear)
        {
            if (!invincibility)
            {
                int temp_health = Game_Manager.Instance.get_health();
                temp_health -= 1;

                if (temp_health > 0)
                {
                    GetComponent<AudioSource>().clip = hitAudio;
                    GetComponent<AudioSource>().pitch = 1.0f;
                    GetComponent<AudioSource>().Play();

                    StartCoroutine(Invincible(invincibilityTime));
                }

                Game_Manager.Instance.set_health(temp_health, "projectile");
            }
        }
    }
    public void OnShoot(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("holdShoot True");
            holdShoot = true;
        }
        //else if (value.canceled)
        //{
        //    Debug.Log("holdShoot False");
        //    holdShoot = false;
        //}
    }
    public void OnJumping(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();

        // Inverts the launching
        if (LaunchToggle.invertedLaunch)
        {
            joystickMovement *= -1;
        }

        // joystick start moving
        if (value.started)
        {
            jumpArrow.SetActive(true);
            lastJoystickVector = Vector2.zero;
        }

        // joysting drag
        if (value.performed)
        {
            if (Vector2.Distance(joystickMovement, Vector2.zero) > 0.3f)
            {
                if (!jumpArrow.activeSelf)
                {
                    jumpArrow.SetActive(true);
                }
                jumpArrow.transform.position = Vector3.MoveTowards(gameObject.transform.position + new Vector3 (joystickMovement.x,joystickMovement.y,0f) * 3, gameObject.transform.position, Time.deltaTime);
                jumpArrow.transform.up = -(transform.position - jumpArrow.transform.position);
            }
            else
            {
                jumpArrow.SetActive(false);
            }

            if (jumpCounter < jumpLimit)
            {
                if (Vector2.Distance(joystickMovement, Vector2.zero) > 0.3f)
                {
                    lastJoystickVector = joystickMovement;
                }
                else
                {
                    lastJoystickVector = Vector2.zero;
                }
            }
        }

        if (value.canceled)
        {
            Debug.Log("Canceled");
        }

        // joystick release
        if (value.canceled)
        {
            jumpArrow.SetActive(false);

            if (lastJoystickVector != Vector2.zero)
            {

                GetComponent<Rigidbody2D>().AddForce(lastJoystickVector * jumpHeight * movementSpeed, ForceMode2D.Impulse);
                GetComponent<AudioSource>().clip = jumpAudio;
                GetComponent<AudioSource>().pitch = 2.0f;
                GetComponent<AudioSource>().Play();

            }
        }
    }

    void Start()
    {
        jumpArrow = transform.GetChild(0).gameObject;
    }

    void Update()
    {   
        if(main_camera.position.x > 299.5){
            idle_movespeed = new Vector2(0f,0f);
        }
        //shoot
        if (holdShoot && Time.time - timeLastProjectile > shootDelay)
        {
            GameObject laser = Instantiate(projectilePrefab, transform.position, transform.rotation);
            laser.transform.Rotate(0, 0, -90);
            laser.transform.localScale = new Vector3(0.5f,0.3f,0.3f);
            laser.GetComponent<SpriteRenderer>().sortingOrder = laser_layer;
            laser.GetComponent<Projectile_controller>().owner = this.tag;
            laser.GetComponent<Projectile_controller>().aimDirection = Vector2.up;
            timeLastProjectile = Time.time;
            holdShoot = false;
        }
        //movement
        transform.Translate((direction+ idle_movespeed) * movementSpeed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, main_camera.position.x - x_clamp, main_camera.position.x +x_clamp), Mathf.Clamp(transform.position.y, main_camera.position.y - y_clamp, main_camera.position.y + y_clamp), transform.position.z);
    }

    IEnumerator Invincible(float invincibilityLength)
    {
        invincibility = true;
        // GetComponent<SpriteRenderer>().material = Game_Manager.Instance.invincibleMat;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.material = Game_Manager.Instance.invincibleMat;
        yield return new WaitForSeconds(invincibilityLength);
        // GetComponent<SpriteRenderer>().material = Game_Manager.Instance.defaultMat;
        sprite.material = Game_Manager.Instance.defaultMat;
        yield return new WaitForSeconds(0.1f);
        invincibility = false;
    }
}
