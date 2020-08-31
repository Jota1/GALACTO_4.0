using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorMouseOver : MonoBehaviour
{
    public Color normalColor;
    public Color mouseOverColor;
    bool mouseOver = false;
    bool clicked;

    void OnMouseEnter ()
    {
        if (StateManager.cameraCraft)
        {
            mouseOver = true;
            GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);

            if (clicked)
            {
                Debug.Log("CLCKED");
            }
        }
    }

    void OnMouseExit ()
    {
        mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_Color", normalColor);
    }

    private void OnMouseDown()
    {
        clicked = true;
    }
}
