using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerEnter : MonoBehaviour
{
    public AI ai;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Veículo"))
        {
            ai.SetFalaAtiva(true);
            Destroy(gameObject);
        }
    }
}

