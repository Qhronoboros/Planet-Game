using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearData : MonoBehaviour
{
    public GameObject counterText;
    public int counter = 0;
    public AudioSource canvasSource;

    private void OnDisable()
    {
        counter = 0;
        counterText.SetActive(false);
    }

    public void CountClearData()
    {
        canvasSource.Play();

        if (!counterText.activeSelf)
        {
            counterText.SetActive(true);
        }

        counter += 1;

        counterText.GetComponent<Text>().text = (4 - counter).ToString();

        if (counter >= 4)
        {
            Debug.Log("Cleared");
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}
