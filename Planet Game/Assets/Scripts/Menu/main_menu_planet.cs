using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main_menu_planet : MonoBehaviour
{   
    public GameObject next;
    public GameObject previous;
    float movespeed = 32f;
    Vector2 nextposition;
    Vector3 previousposition;
    bool next_checked = false;
    bool previous_checked = false;
    public string current;
    int nextsortorder;
    int prevsortorder;
    int tempsortorder;
    public Text planet_text;
    public int stage_index;
    
    // Start is called before the first frame update
    void Start()
    {
        nextposition = next.transform.position;
        previousposition = previous.transform.position;
        nextsortorder = next.GetComponent<SpriteRenderer>().sortingOrder;
        prevsortorder = previous.GetComponent<SpriteRenderer>().sortingOrder;
        if(this.GetComponent<SpriteRenderer>().sortingOrder == 6 && planet_text.text !=  current){
            planet_text.text = current;
        }
        if(SaveGameManager.Instance.check_level_unlocked(stage_index)){
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {   //update location
        if(next_checked && !previous_checked){
            transform.position = Vector2.MoveTowards(transform.position, nextposition, movespeed*Time.deltaTime);
        }else if(!next_checked && previous_checked){
            transform.position = Vector2.MoveTowards(transform.position, previousposition, movespeed*Time.deltaTime);
        }
        //update text and selected
        if(this.GetComponent<SpriteRenderer>().sortingOrder == 6 && planet_text.text !=  current){
            planet_text.text =  current;
            main_menu.selected_stage_index = stage_index;
        }
        //update color
        if(this.GetComponent<SpriteRenderer>().sortingOrder == 6){
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }else{
            this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
        }
        //update locked state
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
        if(SaveGameManager.Instance.check_level_unlocked(stage_index)){
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }else{
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }
    public void nextpressed(){
        this.GetComponent<SpriteRenderer>().sortingOrder = nextsortorder;
        nextposition = next.transform.position;
        next_checked = true;
        previous_checked = false;

    }
    public void prevpressed(){
        this.GetComponent<SpriteRenderer>().sortingOrder = prevsortorder;
        previousposition = previous.transform.position;
        next_checked = false;
        previous_checked = true;

    }
    public void updatenextorder(){
        nextsortorder = next.GetComponent<SpriteRenderer>().sortingOrder;
        prevsortorder = previous.GetComponent<SpriteRenderer>().sortingOrder;
    }

}
