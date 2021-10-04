using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using SaveableObject;

public class SpecificObject : SaveableObject
{   

    // dont use start function cuz it will override starfunction from saveableobject

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Save(int id){
        base.Save(id);
    }
    public override void Load(string[] values){
        base.Load(values);
    }
}
