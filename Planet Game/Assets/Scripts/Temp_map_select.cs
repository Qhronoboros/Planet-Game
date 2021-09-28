using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_map_select : MonoBehaviour
{   
    public GameObject stage_selection_obj ;
    public GameObject setting_obj;
    int pressed_counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressed(){
        pressed_counter+= 1;
        if(pressed_counter == 8){
            pressed_counter = 0;
            stage_selection_obj.SetActive(true);
            setting_obj.SetActive(false);
        }
    }
}
