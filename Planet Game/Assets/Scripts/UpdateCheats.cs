using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCheats : MonoBehaviour
{
    public Text lifeText;
    public Text jumpText;

    public void UpdateText()
    {
        if (Cheats.infLives)
        {
            lifeText.text = "Åá";
        }
        else
        {
            if (GameManager.Instance != null)
            {
                lifeText.text = GameManager.Instance.lifes.ToString();
            }
            else
            {
                lifeText.text = Game_Manager.Instance.lifes.ToString();
            }
        }

        if (jumpText != null)
        {
            if (Cheats.infLaunches)
            {
                jumpText.text = "Åá";
            }
            else
            {
                jumpText.text = (GameManager.Instance.player.GetComponent<PlayerController>().jumpLimit - GameManager.Instance.player.GetComponent<PlayerController>().jumpCounter).ToString();
            }
        }
    }
}
