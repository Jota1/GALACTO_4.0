using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CansadaDessesEfeitos : MonoBehaviour
{
    public static bool BioNoGerador;
    public GameObject panel;
    public AI ai;
    public GameObject panel2;

    private float timer;
    private bool yes;

    // Start is called before the first frame update
    void Start()
    {
        timer = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if(BioNoGerador && !yes)
        {
            yes = true;
            panel.SetActive(true);
            ai.SetFalaAtiva(true);
            BioNoGerador = false;
        }

        if(panel.activeSelf)
        {
            timer -= Time.timeScale;
            if(timer <= 0)
            {
                panel2.SetActive(true);
            }
        }
    }
}
