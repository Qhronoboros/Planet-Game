using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_healthbar : MonoBehaviour
{   
    public Slider slider;
    public GameObject health_text;
    // Start is called before the first frame update
    // healthbar
    public void set_max_health(int health){
        slider.maxValue = health;
        slider.value = health;
    }

    public void set_health(int health){
        slider.value = health;
    }
    public void set_health_text(string hp){
        health_text.GetComponent<Text>().text = hp;
    }
}
