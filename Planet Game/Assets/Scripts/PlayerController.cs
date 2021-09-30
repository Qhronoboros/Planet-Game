using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    public GameObject jumpArrow;
    public Text jumpCounterText;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public AudioClip jumpAudio;
    public AudioClip hitAudio;

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

    public bool flapping = false;
    Coroutine flappingCoroutine;

    public bool invincibility = false;
    public float invincibilityTime = 1.0f;

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
    private void Start()
    {
        resetPlayer();

        GameManager.Instance.SetLifes(GameManager.Instance.lifes);

        // Get arrow gameObject
        jumpArrow = transform.GetChild(0).gameObject;

        // Give gravity script to self
        gravity = gameObject.AddComponent<Gravity>();
        gravity.assignPlanet(mainPlanetObj);
    }

    public void resetPlayer()
    {
        animator.SetBool("Dead", false);
        transform.position = GameManager.Instance.startPos;
        currentMovement = MovementOptions.Default;
        isGrounded = false;
        holdShoot = false;

        // Check for closest planet
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

        GameManager.Instance.cameraController.UpdateCameraSettings(closestPlanet);

        UpdateJumpCounter(0);

        StartCoroutine(Invincible(2.0f));
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
            GameManager.playerDeaths = GameManager.PlayerDeaths.Border;
            GameManager.Instance.set_health(0);
            Debug.Log("Remove Planet");
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
            UpdateJumpCounter(jumpCounter + 1);
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        float joystickMovement = value.ReadValue<Vector2>().x;

        if (joystickMovement < 0)
        {
            horizontalMovement = -1;
            spriteRenderer.flipX = true;
            animator.SetBool("Moving", true);
            //currentMovement = MovementOptions.Left;

        }
        else if (joystickMovement > 0)
        {
            horizontalMovement = 1;
            spriteRenderer.flipX = false;
            animator.SetBool("Moving", true);
            //currentMovement = MovementOptions.Right;
        }
        else
        {
            horizontalMovement = 0;
            animator.SetBool("Moving", false);
            //currentMovement = MovementOptions.Default;
        }
    }

    public void OnJumping(InputAction.CallbackContext value)
    {
        Vector2 joystickMovement = value.ReadValue<Vector2>();
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
                jumpArrow.transform.position = Vector3.MoveTowards(gameObject.transform.position + GameManager.Instance.cameraController.transform.TransformDirection(joystickMovement) * 3, gameObject.transform.position, Time.deltaTime);
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
                if (lastJoystickVector.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (lastJoystickVector.x > 0)
                {
                    spriteRenderer.flipX = false;
                }

                GetComponent<Rigidbody2D>().AddForce((GameManager.Instance.cameraController.transform.TransformDirection(lastJoystickVector)) * jumpHeight * 15, ForceMode2D.Impulse);
                UpdateJumpCounter(jumpCounter + 1);
                GetComponent<AudioSource>().clip = jumpAudio;
                GetComponent<AudioSource>().pitch = 2.0f;
                GetComponent<AudioSource>().Play();

                if (flapping)
                {
                    StopCoroutine(flappingCoroutine);
                    flapping = false;
                }
                flappingCoroutine = StartCoroutine(FlappingAnim());
            }
        }
    }

    // Animating the jump with the flapping animation
    IEnumerator FlappingAnim()
    {
        flapping = true;
        animator.SetBool("Jumping", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Jumping", false);
        flapping = false;
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
        if (collision.collider.gameObject.tag == "Ring")
        {
            Vector2 point = collision.GetContact(0).point;
            GetComponent<Rigidbody2D>().AddForce((new Vector2(transform.position.x, transform.position.y) - point) * collision.gameObject.GetComponentInParent<PlanetScript>().ringLaunch, ForceMode2D.Impulse);

            OnHit("ring");
        }
        else if (collision.gameObject.tag == "Planet")
        {
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            UpdateJumpCounter(0);
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            animator.SetBool("IsGrounded", false);
            isGrounded = false;
        }
    }

    // Player got hit
    public void OnHit(string cause="")
    {
        if (!GameManager.playerDead && !GameManager.Instance.stageClear)
        {
            if (!invincibility)
            {
                int temp_life = GameManager.Instance.get_life();
                temp_life -= 1;

                if (temp_life > 0)
                {
                    GetComponent<AudioSource>().clip = hitAudio;
                    GetComponent<AudioSource>().pitch = 1.0f;
                    GetComponent<AudioSource>().Play();

                    StartCoroutine(Invincible(invincibilityTime));
                }

                GameManager.Instance.set_health(temp_life, cause);
            }
        }
    }

    // Using invincibility shader
    IEnumerator Invincible(float invincibilityLength)
    {
        invincibility = true;
        GetComponent<SpriteRenderer>().material = GameManager.Instance.invincibleMat;
        yield return new WaitForSeconds(invincibilityLength);
        GetComponent<SpriteRenderer>().material = GameManager.Instance.defaultMat;
        yield return new WaitForSeconds(0.1f);
        invincibility = false;
    }

    public void UpdateJumpCounter(int value)
    {
        jumpCounter = value;
        jumpCounterText.text = (jumpLimit - value).ToString();
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
            laser.GetComponent<ProjectileController>().owner = gameObject.tag;
            laser.GetComponent<ProjectileController>().aimDirection = Vector2.up;

            timeLastProjectile = Time.time;
        }


        // Vignette
        if (BorderDetector.borders.Count == 0 && !GameManager.playerDead)
        {
            BorderDetector.intensity = VignetteWarning.calcIntensity(GameManager.Instance.player.GetComponent<PlayerController>().gravity.planetsOrbiting);
        }
        else if (BorderDetector.borders.Count != 0 && !GameManager.playerDead)
        {
            BorderDetector.intensity = 0;
        }

        //Debug.Log(BorderDetector.intensity.ToString() + " " + BorderDetector.borders.Count.ToString() + " " + GameManager.playerDead.ToString());

        GameManager.Instance.vignetteMat.SetColor("_VColor", new Color(1, Mathf.Max(1 - BorderDetector.intensity, 0.0f), Mathf.Max(1 - BorderDetector.intensity, 0.0f), 1));
        GameManager.Instance.vignetteMat.SetFloat("_VRadius", Mathf.Max(1.0f - BorderDetector.intensity * 0.8f, 0.0f));
        GameManager.Instance.vignetteMat.SetFloat("_VSoft", Mathf.Min(BorderDetector.intensity * 2, 1.0f));
    }
}