using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivarBioLouca : MonoBehaviour
{
    public GameObject[] efeitoLouco;
    //public AI ai;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void AtivarEfeito()
    {
        for(int i = 0; i < efeitoLouco.Length; i ++)
        {
            efeitoLouco[i].SetActive(true);
        }
       
        //ai.SetFalaAtiva(true);
    }
   
}
