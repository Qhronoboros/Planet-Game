using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stage_clear_bread : MonoBehaviour
{
    public Text score;
    public Text coin;
    // Start is called before the first frame update
    void OnEnable(){
        score.text = Game_Manager.Instance.getScore().ToString();
        coin.text = Game_Manager.Instance.get_coin().ToString();
    }
}
