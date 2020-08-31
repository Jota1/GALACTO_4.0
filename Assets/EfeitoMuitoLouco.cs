using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoMuitoLouco : MonoBehaviour
{
    public GameObject fade;
    public AI ai;
    //public GameObject carrinhoOuJogador;
    private bool yes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Biomassa.BioAlien && !yes)
        {
           
            fade.SetActive(true);
            //carrinhoOuJogador.SetActive(false);
            ai.SetFalaAtiva(true);
            yes = true;
        }
    }
}
