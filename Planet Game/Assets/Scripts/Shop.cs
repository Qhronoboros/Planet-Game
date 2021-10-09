using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{   
    int shopitemamount = 3;
    public Text coinstext;
    public Text breadtext;
    public GameObject info;
    GameObject itemcost_parent;


    // Start is called before the first frame update
    void Start()
    {   
        coinstext.text = SaveGameManager.Instance.get_coin().ToString();
        breadtext.text = SaveGameManager.Instance.get_bread().ToString();
        for(int i = 1 ; i< shopitemamount+1 ; i++){
            if(!PlayerPrefs.HasKey("item"+i)){
                PlayerPrefs.SetInt("item"+i,0);
            }
            else{
                int temp = PlayerPrefs.GetInt("item"+i);
                if(temp == 1){
                    // GameObject tempgameobj = GameObject.Find("item"+i);
                    GameObject tempgameobjicon = GameObject.Find("item"+i+"/icon");
                    tempgameobjicon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
                    tempgameobjicon.transform.GetChild(0).gameObject.SetActive(true);
                }

            }
        }

    }

    public void buy(){

        float tempcoin = SaveGameManager.Instance.get_coin();
        float tempbread = SaveGameManager.Instance.get_bread();
        string item = EventSystem.current.currentSelectedGameObject.transform.parent.name;
        float item_cost;
        if(GameObject.Find(item+"/currency/coin") == null){
            itemcost_parent = GameObject.Find(item+"/currency/bread");
            item_cost = float.Parse(itemcost_parent.GetComponent<Text>().text);
            if(item_cost < tempbread && PlayerPrefs.GetInt(item) == 0){
                SaveGameManager.Instance.save_bread( tempbread - item_cost);
                breadtext.text = SaveGameManager.Instance.get_bread().ToString();
                PlayerPrefs.SetInt(item,1);
                GameObject tempgameobjicon = GameObject.Find(item+"/icon");
                tempgameobjicon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
                tempgameobjicon.transform.GetChild(0).gameObject.SetActive(true);
            }else if(item_cost > tempbread && PlayerPrefs.GetInt(item) == 0){
                info.transform.GetChild(0).GetComponent<Text>().text = "insufficient bread";
                info.gameObject.SetActive(true);
            }

        }else{

            itemcost_parent = GameObject.Find(item+"/currency/coin");
            item_cost = float.Parse(itemcost_parent.GetComponent<Text>().text);
            if(item_cost < tempcoin && PlayerPrefs.GetInt(item) == 0){
                SaveGameManager.Instance.save_coin( tempcoin - item_cost);
                coinstext.text = SaveGameManager.Instance.get_coin().ToString();
                PlayerPrefs.SetInt(item,1);
                GameObject tempgameobjicon = GameObject.Find(item+"/icon");
                tempgameobjicon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
                tempgameobjicon.transform.GetChild(0).gameObject.SetActive(true);

            }else if(item_cost > tempcoin && PlayerPrefs.GetInt(item) == 0){
                info.transform.GetChild(0).GetComponent<Text>().text = "insufficient coin";
                info.gameObject.SetActive(true);                
            }
            
        }
    }

    public void show_description(){
        // string item = EventSystem.current.currentSelectedGameObject.transform.parent.name;
        // GameObject.Find(item+"/currency/coin")
        string description = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
        info.transform.GetChild(0).GetComponent<Text>().text = description;
        info.gameObject.SetActive(true); 
    }
}
