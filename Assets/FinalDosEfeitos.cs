using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDosEfeitos : MonoBehaviour
{
    public GameObject EfeitosChatos;
    public GameObject FinalDoJogoUI;
   
    public void CancelarEfeito()
    {
        FinalDoJogoUI.SetActive(true);
        Destroy(EfeitosChatos);
    }
}
