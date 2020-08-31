using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancelar_FalaEAnim : MonoBehaviour
{
    public AI ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cancelar()
    {
        ai.CancelarFala();
        gameObject.SetActive(false);
    }
}
