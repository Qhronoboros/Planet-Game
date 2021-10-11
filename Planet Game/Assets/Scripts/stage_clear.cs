using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stage_clear : MonoBehaviour
{   
    public Text score;
    public Text bread;
    public Text star;
    // Start is called before the first frame update
    void OnEnable(){
        score.text = GameManager.Instance.getScore().ToString();
        bread.text = GameManager.Instance.get_bread().ToString();
        star.text = GameManager.Instance.get_star().ToString() + "/ " + "1";
    }
}
