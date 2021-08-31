using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject planet;
    public static MovementOptions currentMovement = MovementOptions.Default;
    public float movementSpeed = 2.0f;
    public float jumpHeight = 0.05f;
    public float distance = 0;

    public enum MovementOptions
    {
        Default,
        Left,
        Right
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, planet.transform.position);

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentMovement = MovementOptions.Left;
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentMovement = MovementOptions.Right;
        }

        if (currentMovement == MovementOptions.Left && Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(planet.transform.position, Vector3.forward, movementSpeed / (distance * 0.2f) * Time.deltaTime);
        }
        else if (currentMovement == MovementOptions.Right && Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(planet.transform.position, Vector3.back, movementSpeed / (distance * 0.2f) * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space) && distance < 20)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.MoveTowards(transform.position, planet.transform.position, 0) * jumpHeight * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
