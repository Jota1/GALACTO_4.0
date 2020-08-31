using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFala : MonoBehaviour
{
    public AI ai;
    public GameObject barracaBussola;
    private float timer;
    private bool falainicial;

    private void Start()
    {
        falainicial = false;
        timer = 15;
    }

    private void Update()
    {
        timer -= Time.timeScale;

        if(timer <= 0 && !falainicial)
        {
            ai.SetFalaAtiva(true);
            falainicial = true;
            barracaBussola.SetActive(true);
            Destroy(gameObject);
        }
    }
}