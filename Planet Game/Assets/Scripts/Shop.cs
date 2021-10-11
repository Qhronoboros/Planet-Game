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

    public AudioSource sufficientTrack;
    public AudioSource insufficientTrack;

    // Start is called before the first frame update
    void Start() {
        // PlayerPrefs.DeleteAll();
        
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

    // Refresh Text
    private void OnEnable()
    {
        if (Cheats.infCoins)
        {
            coinstext.text = "∞";
        }
        else
        {
            coinstext.text = SaveGameManager.Instance.get_coin().ToString();
        }

        if (Cheats.infBread)
        {
            breadtext.text = "∞";
        }
        else
        {
            breadtext.text = SaveGameManager.Instance.get_bread().ToString();
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
            if(PlayerPrefs.GetInt(item) == 0 && (item_cost < tempbread || Cheats.infCoins)){
                if (!Cheats.infCoins)
                {
                    SaveGameManager.Instance.save_bread(tempbread - item_cost);
                    breadtext.text = SaveGameManager.Instance.get_bread().ToString();
                }

                sufficientTrack.Play();
                PlayerPrefs.SetInt(item,1);
                GameObject tempgameobjicon = GameObject.Find(item+"/icon");
                tempgameobjicon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
                tempgameobjicon.transform.GetChild(0).gameObject.SetActive(true);
            }else if(item_cost > tempbread && PlayerPrefs.GetInt(item) == 0){
                insufficientTrack.Play();
                info.transform.GetChild(0).GetComponent<Text>().text = "insufficient bread";
                info.gameObject.SetActive(true);
            }

        }else{

            itemcost_parent = GameObject.Find(item+"/currency/coin");
            item_cost = float.Parse(itemcost_parent.GetComponent<Text>().text);
            if(PlayerPrefs.GetInt(item) == 0 && (item_cost < tempcoin || Cheats.infBread)){
                if (!Cheats.infBread)
                {
                    SaveGameManager.Instance.save_coin(tempcoin - item_cost);
                    coinstext.text = SaveGameManager.Instance.get_coin().ToString();
                }

                sufficientTrack.Play();
                PlayerPrefs.SetInt(item,1);
                GameObject tempgameobjicon = GameObject.Find(item+"/icon");
                tempgameobjicon.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 255f);
                tempgameobjicon.transform.GetChild(0).gameObject.SetActive(true);

            }else if(item_cost > tempcoin && PlayerPrefs.GetInt(item) == 0){
                insufficientTrack.Play();
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
