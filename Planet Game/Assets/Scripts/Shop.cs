using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{   
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public Text coinstext;
    public Text breadtext;
    GameObject itemcost_parent;
    // public GameObject item1;
    // public GameObject item1;
    // public GameObject item1;

    // Start is called before the first frame update
    void Start()
    {   
        coinstext.text = SaveGameManager.Instance.get_coin().ToString();
        breadtext.text = SaveGameManager.Instance.get_bread().ToString();
        for(int i = 1 ; i< 4 ; i++){
            if(!PlayerPrefs.HasKey("item1"+i)){
                PlayerPrefs.SetInt("item"+i,0);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buy(){
        float tempcoin = SaveGameManager.Instance.get_coin();
        float tempbread = SaveGameManager.Instance.get_bread();
        string item = EventSystem.current.currentSelectedGameObject.transform.parent.name;
        float item_cost;
        if(GameObject.Find(item+"/currency/coin") == null){
            itemcost_parent = GameObject.Find(item+"/currency/bread");
            item_cost = float.Parse(itemcost_parent.GetComponent<Text>().text);
            if(item_cost < tempbread){
                SaveGameManager.Instance.save_coin( tempbread - item_cost);
                coinsbread.text = SaveGameManager.Instance.get_bread().ToString();
            }

        }else{
            itemcost_parent = GameObject.Find(item"/currency/coin");
            item_cost = float.Parse(itemcost_parent.GetComponent<Text>().text);
            if(item_cost < tempcoin){
                SaveGameManager.Instance.save_coin( tempcoin - item_cost);
                coinstext.text = SaveGameManager.Instance.get_coin().ToString();
            }
            
        }
    }
}
