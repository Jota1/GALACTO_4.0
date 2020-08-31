using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generators : MonoBehaviour
{
    public static int MaxCapacity;
    public static int currentEnergy ;

    private void Start()
    {
        MaxCapacity = 0;
        currentEnergy = 0;
        UpdateInterface.instance.Update2();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
