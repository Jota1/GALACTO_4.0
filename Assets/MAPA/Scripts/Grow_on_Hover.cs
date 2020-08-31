using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grow_on_Hover : MonoBehaviour
{
    Image icon;

    public void Start()
    {
        icon = GetComponent<Image>();
        icon.rectTransform.sizeDelta = new Vector2(50, 60);
    }

    public void Grow()
    {
        icon.rectTransform.sizeDelta = new Vector2(110, 130);
    }
    public void Small()
    {
        icon.rectTransform.sizeDelta = new Vector2(50, 60);
    }
}
