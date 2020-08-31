using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FimDoJogo : MonoBehaviour
{
    private float timer;
    public Text timeText;
    public GameObject[] outrasInterfaces;

    // Start is called before the first frame update
    void Start()
    {
        timer = 20;

        for (int i = 0; i < outrasInterfaces.Length; i++)
        {
            outrasInterfaces[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        timeText.text = "" + (int)timer;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
