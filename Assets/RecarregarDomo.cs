using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecarregarDomo : MonoBehaviour
{
    public GameObject recarregarUI;
    public GameObject[] otherUIElements;
    public Dome dome;
    public GameObject Domo;
    public GameObject Text;
    public AI Liz;
    private bool falinha;


    public void AtivarUI()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Text.SetActive(false);
        UpdateInterface.CursorLock = false;

        Time.timeScale = 0;

        recarregarUI.SetActive(true);


        for (int i = 0; i < otherUIElements.Length; i++)
        {
            otherUIElements[i].gameObject.SetActive(false);
        }
    }

    public void DesativarUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       UpdateInterface.CursorLock = true;

        Time.timeScale = 1;

        recarregarUI.SetActive(false);

        for (int i = 0; i < otherUIElements.Length; i++)
        {
            otherUIElements[i].gameObject.SetActive(true);
        }
    }

    public void carregar()
    {
        if (Generators.currentEnergy >= 5)
        {
            dome.SetEnergiaDoDomo();
            if (!Domo.activeSelf)
            {
                Domo.SetActive(true);
            }
            Generators.currentEnergy -= 5;
            UpdateInterface.instance.Update2();

            if(!falinha)
            {
                Liz.SetFalaAtiva(true);
            }
            DesativarUI();
        }

        else Text.SetActive(true);
    }
}
