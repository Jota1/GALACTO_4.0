using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesativarCigarro : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        timer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 5;

           

            if (Generators.currentEnergy > Generators.MaxCapacity)
            {
                Generators.currentEnergy = Generators.MaxCapacity;
            }

         

            gameObject.SetActive(false);

        }
    }

}
