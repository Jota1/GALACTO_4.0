using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class AtivarTriggers : MonoBehaviour
{
    public AI ai;
    public GameObject[] triggers;
    public GameObject[] barreiraProJogador;
    public GameObject[] cercasFantasmas;

    public static bool cercasAtivadas;

    // Start is called before the first frame update
    void Start()
    {
        cercasAtivadas = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(ai.GetnarrativaAtual() == 2 && !triggers[0].activeInHierarchy)
        {
            triggers[0].SetActive(true);
            barreiraProJogador[0].SetActive(false);
            barreiraProJogador[1].SetActive(false);


        }

        if(ai.GetnarrativaAtual() == 3 && !cercasAtivadas)
        {
            ai.SetDanoInicial(false);
            barreiraProJogador[0].SetActive(true);
            barreiraProJogador[1].SetActive(true);

            for (int i = 0; i < cercasFantasmas.Length; i++)
            {
                cercasFantasmas[i].SetActive(true);
            }

            cercasAtivadas = true;
        }

        if (ai.GetnarrativaAtual() == 4 && !ai.GetFalaAtiva())
        {
            barreiraProJogador[0].SetActive(false);
            barreiraProJogador[1].SetActive(false);
            barreiraProJogador[2].SetActive(false);
            triggers[1].SetActive(true);

        }

        if (ai.GetnarrativaAtual() == 5 && !ai.GetFalaAtiva())
        {
            ai.SetFalaAtiva(true);
        }

        if (ai.GetnarrativaAtual() == 7 && !ai.GetFalaAtiva())
        {
            ai.SetFalaAtiva(true);
        }

        if (ai.GetnarrativaAtual() == 8 && !ai.GetFalaAtiva())
        {
            ai.SetFalaAtiva(true);
        }


       
    }
}
