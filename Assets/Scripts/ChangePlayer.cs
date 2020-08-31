using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChangePlayer : MonoBehaviour
{
    public static bool playerBool;
    public GameObject player;
    public GameObject car;
    public Transform carPosition;
    public GameObject car2;
    public Collider collider;


    InventoryUI inventoryUI;
    Inventory inventory;

    private void Start()
    {
        playerBool = true;
        inventory = Inventory.instance;
        inventoryUI = InventoryUI.instance;
        inventory.onItemChangedCallback += inventoryUI.UpdateUI;
    }

  
    public void ChangePlayerInteract()
    {
      
         FuelSystem.StartFuel = 100;
         UpdateInterface.instance.Update2();
         

       if(playerBool)
        {
            Debug.Log("hue");
            Inventory.currentInventory = "Car";
            inventory.changeInventory(Inventory.currentInventory);
            InventoryUI.instance.ChangeInventory();
        
            car.SetActive(true);
            car2.SetActive(false);
            player.SetActive(false);
            playerBool = false;
            collider.enabled = false;
        }

       else
        {

            Inventory.currentInventory = "Player";
            inventory.changeInventory(Inventory.currentInventory);
            InventoryUI.instance.ChangeInventory();
          
            player.SetActive(true);
            car2.transform.position = carPosition.position;
            car2.transform.rotation = carPosition.rotation;
            car2.SetActive(true);
            car.SetActive(false);
            playerBool = true;
        }
    }

    public void Update()
    {
        // Debug.Log(FuelSystem.StartFuel);
        // if(!playerBool && FuelSystem.startFuel <= 0)
        //{
        //    ChangePlayerInteract();
        // }
        if (Cheats.poderVoltarAoVeículo)
        {
            collider.enabled = false;
        }

        if (!collider.enabled && Input.GetKeyDown(KeyCode.V) && Time.timeScale != 0)
        {
            ChangePlayerInteract();
        }

        
    }
}
