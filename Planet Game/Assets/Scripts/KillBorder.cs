using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBorder : MonoBehaviour
{
    public PlanetScript parentInstance;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.player.GetComponent<PlayerController>().AddPlanet(parentInstance.gameObject);
            GameManager.Instance.player.GetComponent<PlayerController>().ChangePlanet(parentInstance.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.player.GetComponent<PlayerController>().RemovePlanet(parentInstance.gameObject);
        }
    }
}
