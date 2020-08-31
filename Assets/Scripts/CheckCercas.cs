using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCercas : MonoBehaviour
{
    public GameObject Cerca1;
    public GameObject Cerca2;
    public GameObject Cerca3;
    public GameObject Cerca4;
    public GameObject Cerca5;
    public GameObject Cerca6;
    public GameObject Domo;
    private bool Check;
    public GameObject ChuvaInicial;
    public AI ai;
    public GameObject CameraDomo;
    public GameObject RecarregarDomo;


    // Start is called before the first frame update
    void Start()
    {
        Check = false;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!Check)
        {
            if (Cerca1.activeSelf && Cerca2.activeSelf && Cerca3.activeSelf && Cerca4.activeSelf && Cerca5.activeSelf && Cerca6.activeSelf)
            {

                Domo.SetActive(true);
                RecarregarDomo.SetActive(true);
                ChuvaInicial.SetActive(false);
                CameraDomo.SetActive(true);
                ai.SetFalaAtiva(true);

                Check = true;
            }

        }

      

    }
}
