using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public abstract class SaveableObject : MonoBehaviour
{

    //types of objects
    enum ObjectType
    {   //dont use _ for objecttype
        Coins , Enemies , Star, CoinIntro, Bread, Stage1Fragm, Stage2Fragm ,Stage3Fragm
    }

    protected string save;

    [SerializeField]
    private ObjectType objectType;

    // Start is called before the first frame update
    void Start()
    {
        SaveGameManager.Instance.SaveableObjects.Add(this);
        // PlayerPrefs.SetInt("age",30);
    }

    public virtual void Save(int id){

        PlayerPrefs.SetString(SceneManager.GetActiveScene().buildIndex + "-" + id.ToString(), objectType + "_" + transform.position.ToString());

    }
    public virtual void Load (string[] values){

        transform.localPosition = SaveGameManager.Instance.StringToVector(values[1]);

    }
    public void DestroySaveable(){
        SaveGameManager.Instance.SaveableObjects.Remove(this);
    }
}
