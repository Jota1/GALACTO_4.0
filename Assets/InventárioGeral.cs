using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventárioGeral : MonoBehaviour
{

    public GameObject InventoryPlayerUI;
    public GameObject InventoryCarUI;
    public GameObject[] otherUIElements;
    public Transform Pos;
    private Vector3 PosInicial;
    public Transform PosNova;

    // Start is called before the first frame update
    void Start()
    {

        PosInicial = Pos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && InventoryPlayerUI.activeSelf == false && Time.timeScale != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UpdateInterface.CursorLock = false;

            Time.timeScale = 0;

            InventoryCarUI.transform.position = PosNova.position;
            InventoryPlayerUI.SetActive(true);
            InventoryCarUI.SetActive(true);


            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(false);
            }
        }

        else if (Input.GetKeyDown(KeyCode.N) && InventoryPlayerUI.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UpdateInterface.CursorLock = true;

            Time.timeScale = 1;

            InventoryCarUI.transform.position = PosInicial;
            InventoryPlayerUI.SetActive(false);
            InventoryCarUI.SetActive(false);

          

            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
        }
    }
}
