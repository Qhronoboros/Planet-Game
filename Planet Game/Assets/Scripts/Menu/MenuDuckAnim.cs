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
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = ducky.GetComponent<Rigidbody2D>();
	}
	
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
				rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				if (rb.velocity.y > 100.0f) { rb.velocity = new Vector2(0, 100.0f); }
			}
			yield return null;
		}
	}
}
