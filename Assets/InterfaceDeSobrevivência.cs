using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceDeSobrevivência : MonoBehaviour
{
    public GameObject UI;
    public GameObject[] otherUIElements;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.O) && UI.activeSelf == false && Time.timeScale != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateInterface.CursorLock = false;

            Time.timeScale = 0;

            UI.SetActive(true);


            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.O) && UI.activeSelf == true || Input.GetKeyDown(KeyCode.Escape) && UI.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UpdateInterface.CursorLock = true;

            Time.timeScale = 1;

            UI.SetActive(false);

            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
        }


    }
}
