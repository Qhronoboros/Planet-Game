using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager instance;
    public List<SaveableObject> SaveableObjects {get; private set;}
    public int totalstages = 5;

    public static SaveGameManager Instance
    {
        get
        {   
            if(instance == null)
            {
                instance= GameObject.FindObjectOfType<SaveGameManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        SaveableObjects = new List<SaveableObject>();
        if (!check_save_exist()){
            create_new_save();
        }
        
        // clear_data();
    }

    public void Save(){
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), SaveableObjects.Count);

        for (int i = 0; i < SaveableObjects.Count; i++){
            SaveableObjects[i].Save(i);
        }
    }

    public void Load(){

        foreach ( SaveableObject obj in SaveableObjects)
        {
            if (obj != null){
                Destroy(obj.gameObject);
            }
        }

        SaveableObjects.Clear();

        int objectCount = PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex.ToString());

        for (int i = 0; i < objectCount; i++){
            string[] value = PlayerPrefs.GetString(SceneManager.GetActiveScene().buildIndex + "-" + i.ToString()).Split('_');
            GameObject temp = null;
            GameObject parent_of_obj = null;
            switch (value[0])
            {
                case "Coins":
                    print("yeaaaa here");
                    temp = Instantiate(Resources.Load("Prefabs/coin")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/coins");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Star":
                    temp = Instantiate(Resources.Load("Prefabs/Star")) as GameObject;
                    break;
                case "Enemies":
                    temp = Instantiate(Resources.Load("Prefabs/Fox Enemy")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/enemies");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "planet_piece":
                    temp = Instantiate(Resources.Load("Prefabs/special")) as GameObject;
                    break;

            }
            if (temp != null){
                temp.GetComponent<SaveableObject>().Load(value);
            }
        }
    }
    
    public Vector3 StringToVector(string value){
        value = value.Trim(new char[] {'(',')'});
        value = value.Replace(" ", "");
        string[] pos = value.Split(',');
        return new Vector3(float.Parse(pos[0]),float.Parse(pos[1]),float.Parse(pos[2]));
    }
    // public Quaternion StringToQuaternion(string value){
    //     return Quaternion.identity;
    // }
    public void clear_data (){
        PlayerPrefs.DeleteAll();
    }

    public void get_level_unlocks(){
        for(int i=0;i<totalstages;i++){
            print(PlayerPrefs.GetInt("scene"+i));
        }
    }

    public bool check_level_unlocked(int index){
        int temp = PlayerPrefs.GetInt("scene"+index);
        if(temp == 1){
            return true;
        }
        else{
            return false;
        }
    }
    
    public void unlock_level(int level){
        PlayerPrefs.SetInt("scene"+level,1);
    }
    public void get_score(){

    }
    public float get_coin(){
        return PlayerPrefs.GetFloat("coin");
    }

    public void save_coin(float amount){
        PlayerPrefs.SetFloat("coin",amount);
    }
    public void save_score(){

    }
    
    public bool check_save_exist(){
        return PlayerPrefs.HasKey("save_exist");
    }

    public bool check_stage_saved(){
        return PlayerPrefs.HasKey(SceneManager.GetActiveScene().name);
    }

    public void create_new_save(){
        //make sure to delete all saved playerpref
        PlayerPrefs.DeleteAll();
        //for check if its a new game
        PlayerPrefs.SetInt("save_exist", 1);
        //======level unlocks====
        PlayerPrefs.SetInt("scene0",1);
        for(int i=1;i<totalstages;i++){
            PlayerPrefs.SetInt("scene"+i,0);
            print(i);
        }
        //======coin=============
        save_coin(0f);
        print("save game created");
    }


}
