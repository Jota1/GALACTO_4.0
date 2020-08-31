using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivarCoisas : MonoBehaviour
{
    public GameObject Bioefeito;
    public GameObject carrinhoOuJogador;
    public AI ai;

    // Start is called before the first frame update
  
    public void cancelarEfeito()
    {
        carrinhoOuJogador.SetActive(true);
        ai.SetFalaAtiva(true);
        Destroy(Bioefeito);
    }
}
