
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Biomassa : MonoBehaviour
{

    private Inventory inventory;
    public Item item;
    public int amount;
    public static bool BioAlien;
    public GameObject fumaça;
    public GameObject Crystal;
    private bool CrystalAtivo;
   
    private float timer = 60;

    // Start is called before the first frame update
    void Start()
    {
        BioAlien = false;
        Generators.MaxCapacity += 10;
        UpdateInterface.instance.Update2();
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!CrystalAtivo && CansadaDessesEfeitos.BioNoGerador)
        {
            Crystal.SetActive(true);
            CrystalAtivo = false;
        }
      
        if(BioAlien)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {

                timer = 60;
                Generators.currentEnergy += 5;
              
                if(Generators.currentEnergy > Generators.MaxCapacity)
                {
                    Generators.currentEnergy = Generators.MaxCapacity;
                }
                UpdateInterface.instance.Update2();
            }
        }
    }

    public void GenerateEnergy()
    {
        if (HasMaterials(inventory) && Generators.currentEnergy < Generators.MaxCapacity)
        {

            RemoveMaterials(inventory);

            Generators.currentEnergy += 5;
            TextTime.feedbackString = "- Estrume";
            TextTime.textAtivado = true;
            fumaça.SetActive(true);
            UpdateInterface.instance.Update2();

        }

    }

    public bool HasAll()
    {
        if (HasMaterials(inventory) && Generators.currentEnergy < Generators.MaxCapacity)
        {
            return true;
        }

        else return false;
    }

    public bool HasMaterials(ItemContainer itemContainer)
    {


        if (itemContainer.ItemCount(item.name, "Player") < 1)
        {
            Debug.LogWarning("You don't have the required materals.");
            return false;
       }

       return true;
    }

    public void RemoveMaterials(ItemContainer itemContainer)
    {

        //for (int i = 0; i < amoun; i++)
       // {
            Inventory.instance.Remove(item, "Player");
       // }
    }

}
