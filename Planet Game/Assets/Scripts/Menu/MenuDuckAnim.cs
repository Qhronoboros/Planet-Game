using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDuckAnim : MonoBehaviour
{
    public GameObject ducky;
    public GameObject pluto;

    public float jumpForce = 5.0f;
    public static bool isGrounded = false;
    public Vector2 startPos;
    public Coroutine jumpCoroutine;

    void Update()
    {
        pluto.transform.Rotate(0, 0, 50 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }

    private void OnEnable()
    {
        jumpCoroutine = StartCoroutine(DuckJump());
    }

    private void OnDisable()
    {
        StopCoroutine(jumpCoroutine);
    }

    IEnumerator DuckJump()
    {
        while (true)
        {
            if (isGrounded)
            {
                ducky.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            yield return null;
        }
    }
}
