using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_rotation : MonoBehaviour
{
    public void rotate_bg(){
        for(int i = 0 ; i < transform.childCount ; i ++){
            if (transform.GetChild(i).GetComponent<Background>().state == "last"){
                if(transform.GetChild(i).GetComponent<SpriteRenderer>().flipX){
                    transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = false;
                }else{
                    transform.GetChild(i).GetComponent<SpriteRenderer>().flipX = true;
                }
                transform.GetChild(i).transform.position = new Vector3(transform.GetChild(i).transform.position.x + 60f , 0f , 0f);
                transform.GetChild(i).GetComponent<Background>().state = "next";
                continue;
            }
        }
    }
}
