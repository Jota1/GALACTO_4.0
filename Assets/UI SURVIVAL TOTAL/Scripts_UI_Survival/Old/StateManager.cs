using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static bool cameraCraft;
    public Camera normalCamera;
    public Camera craftCamera;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraCraft = true;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            cameraCraft = false;
        }


        if (cameraCraft)
        {
            craftCamera.depth = 2;
            normalCamera.depth = -2;
        }
        else { craftCamera.depth = -2; normalCamera.depth = 2;}
    }
}
