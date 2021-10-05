using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public void reset_data(){
        PlayerPrefs.DeleteAll();
        print("data cleared");
    }
}
