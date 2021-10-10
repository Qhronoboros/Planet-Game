using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Globalization;

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
                instance = FindObjectOfType<SaveGameManager>();
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
        // if(check_stage_saved()){
        //     StartCoroutine(delayLoad());
        // }
        
        // clear_data();
    }

    void Start(){
        if(check_stage_saved()){
            Load();
        }
    }

    public void Save(){
        print("saveddddd");
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex.ToString(), SaveableObjects.Count);

        for (int i = 0; i < SaveableObjects.Count; i++){
            SaveableObjects[i].Save(i);
        }
    }

    public void Load(){
        print("loaded");

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
                    temp = Instantiate(Resources.Load("Prefabs/coin")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/coins");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Star1":
                    temp = Instantiate(Resources.Load("Prefabs/Pluto Star")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/stars");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Star2":
                    temp = Instantiate(Resources.Load("Prefabs/Saturn Star")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/stars");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Star3":
                    temp = Instantiate(Resources.Load("Prefabs/Sun Star")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/stars");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Fox":
                    temp = Instantiate(Resources.Load("Prefabs/Fox Enemy")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/enemies");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Cat":
                    temp = Instantiate(Resources.Load("Prefabs/Cat Enemy")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/enemies");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Dog":
                    temp = Instantiate(Resources.Load("Prefabs/Dog Enemy")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/enemies");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;    
                case "Stage1Fragm":
                    temp = Instantiate(Resources.Load("Prefabs/pluto special")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/special_obj");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Stage2Fragm":
                    temp = Instantiate(Resources.Load("Prefabs/Saturn special")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/special_obj");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Stage3Fragm":
                    temp = Instantiate(Resources.Load("Prefabs/Sun special")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/special_obj");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "CoinIntro":
                    temp = Instantiate(Resources.Load("Prefabs/intro/coin_intro")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/Coins");
                    temp.transform.SetParent(parent_of_obj.transform);
                    break;
                case "Bread":
                    temp = Instantiate(Resources.Load("Prefabs/bread")) as GameObject;
                    parent_of_obj = GameObject.Find("Stage/stage_obj/breads");
                    temp.transform.SetParent(parent_of_obj.transform);
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
        return new Vector3(float.Parse(pos[0], CultureInfo.InvariantCulture),float.Parse(pos[1], CultureInfo.InvariantCulture),float.Parse(pos[2], CultureInfo.InvariantCulture));
    }
    // public Quaternion StringToQuaternion(string value){
    //     return Quaternion.identity;
    // }
    public void clear_data (){
        PlayerPrefs.DeleteAll();
        print("data cleared");
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
    public float get_bread(){
        return PlayerPrefs.GetFloat("bread");
    }

    public void save_bread(float amount){
        PlayerPrefs.SetFloat("bread",amount);
    }
    public void save_score(){

    }
    
    public bool check_save_exist(){
        return PlayerPrefs.HasKey("save_exist");
    }

    public bool check_stage_saved(){
        return PlayerPrefs.HasKey(SceneManager.GetActiveScene().name);
    }

    public bool check_muted(){
        if(PlayerPrefs.GetInt("Sound") == 1){
            return true;
        }else{
            return false;
        }
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
        save_bread(0f);
        print("save game created");
    }

    IEnumerator delayLoad()
    {
        yield return new WaitForSeconds(0.005f);
        Load();
    }


}
