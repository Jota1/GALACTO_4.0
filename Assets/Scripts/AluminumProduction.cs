using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AluminumProduction : MonoBehaviour
{
    public Animator anim;
    public GameObject aluminum;
    public Transform positionAluminum;
    public GameObject bauxite;

    public CraftingRecipe craftingRecipe;
    public Inventory inventory;

    public static bool AluminioProduzido = false;

    private void Start()
    {
        AluminioProduzido = false;
        //Generators.currentEnergy = 20;
       // UpdateInterface.instance.Update2();
    }

    public void ProduzirAluminum()
    {
        if(craftingRecipe.CraftAluminum(inventory))
        {
            Instantiate(aluminum, positionAluminum.position, Quaternion.identity);
            AluminioProduzido = true;
            anim.SetTrigger("Refinando");
            bauxite.SetActive(false);
            bauxite.SetActive(true);
        }
       
    }

    public bool HasMaterial()
    {
        if (craftingRecipe.HasMaterials(inventory) && craftingRecipe.HasEnergia() && !AluminioProduzido)
        {
            return true;
        }

        else return false;
    }

}
