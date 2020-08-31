using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFeatures : MonoBehaviour
{

    CombinedShadows fastShadows;
    public static int selection = 0;

    // Use this for initialization
    void Start()
    {
        fastShadows = this.GetComponent<CombinedShadows>();
    }

    // Update is called once per frame
    void Update()
    {
        bool buttonPressed = Input.GetButtonDown("Xbox_A");
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began ) ||
            buttonPressed )
        {
            selection = (selection + 1) % 2;
        }

        //Debug.Log("Xbox_Y  = " + buttonPressed);
        fastShadows.enabled = (selection == 0);
    }

    public static string GetCurrentFeatureString()
    {
        switch (selection)
        {
            case 0: return "Rendering with CombinedShadows"; break;
            case 1: return "Rendering without CombinedShadows"; break;
        }

        return "error";
    }
}