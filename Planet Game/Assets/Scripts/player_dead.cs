using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_dead : MonoBehaviour
{
    public Image img;
    public GameObject retry;
    public GameObject quit;
    public GameObject text;

    void OnEnable()
    {
        StartCoroutine(FadeImage(false));
    }
 
    IEnumerator FadeImage(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                retry.GetComponent<Image>().color = new Color(1, 1, 1, i);
                quit.GetComponent<Image>().color = new Color(1, 1, 1, i);
                text.GetComponent<Text>().color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}
