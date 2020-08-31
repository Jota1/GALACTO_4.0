using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingGenerator : MonoBehaviour
{
	public CraftingRecipe craftingRecipe;
	public GameObject generatorPreview;
	public GameObject constructionUI;
	public GameObject generatorImageUI;
	public GameObject[] otherUIElements;


	public NovoSistemaDeConstrução buildSystem;
	public Inventory inventory;
	public Text GeneratorQuantText;
	public Button constructButton;
	public Button produzir;
	public static int GeneratorQuant;


	public void Update()
	{
		if(craftingRecipe.HasMaterials(inventory) && !produzir.interactable)
		{
			produzir.interactable = true;
		}

		else if (!craftingRecipe.HasMaterials(inventory))
		{
			produzir.interactable = false;
		}
	}
	public void CraftGenerator()
	{
		if (craftingRecipe != null && inventory != null)
		{
			GeneratorQuant += craftingRecipe.CraftGenerator(inventory);
			GeneratorQuantText.text = "" + GeneratorQuant;

			if (GeneratorQuant > 0)
			{
				constructButton.interactable = true;
			}
		}

	}

	public void ConstructionGenerator()
	{
		if (!buildSystem.IsBuilding)
		{
			buildSystem.NewBuild(generatorPreview);
			GeneratorQuant -= 1;
			GeneratorQuantText.text = "" + GeneratorQuant;
			constructionUI.SetActive(false);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Time.timeScale = 1;
			UpdateInterface.CursorLock = true;

			if (GeneratorQuant == 0)
			{
				constructButton.interactable = false;
			}

			for (int i = 0; i < otherUIElements.Length; i++)
			{
				otherUIElements[i].gameObject.SetActive(true);
			}

			generatorImageUI.SetActive(false);

		}
	}
}
