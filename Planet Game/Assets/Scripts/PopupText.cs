using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    public string text;
    public float startTime = 2.0f;
    public float endTime = 5.0f;

    void Start()
    {
        if (name == "Start Text")
        {
            text = "Collect " + GameManager.Instance.max_special.ToString() + " planet pieces";
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(startTime);
        GetComponent<Text>().text = text;
        yield return new WaitForSeconds(endTime);
        gameObject.SetActive(false);
    }
}
