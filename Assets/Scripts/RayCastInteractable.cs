using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastInteractable : MonoBehaviour
{
	public float weaponRange;
	private Camera fpsCam;
	public Texture2D crosshairE;
	public Texture2D crosshairLaser;
	public Texture2D crosshairBact;
	public Texture2D crosshairHand;
	public Texture2D crosshair;
	public LayerMask layer;

	private Texture2D texture;

	private void Start()
	{
		texture = crosshair;
		fpsCam = GetComponentInParent<Camera>();
	}

	private void Update()
	{
		

		Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));


		RaycastHit hit;


		if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, layer))
		{

			PlasticCollect collect = hit.collider.GetComponent<PlasticCollect>();
			PlasticDeposit deposit = hit.collider.GetComponent<PlasticDeposit>();
			AluminumProduction aluminum = hit.collider.GetComponent<AluminumProduction>();
			ItemPickup pickup = hit.collider.GetComponent<ItemPickup>();
			ItemBact bact = hit.collider.GetComponent<ItemBact>();
			Crafting craft = hit.collider.GetComponent<Crafting>();
			Biomassa bio = hit.collider.GetComponent<Biomassa>();
			Converter converter = hit.collider.GetComponent<Converter>();
			ChangePlayer change = hit.collider.GetComponent<ChangePlayer>();
			Cut itemCut = hit.collider.GetComponent<Cut>();
			RecarregarDomo domoRecarga = hit.collider.GetComponent<RecarregarDomo>();


			if (collect != null || deposit != null || bact != null || craft != null || bio != null || converter != null || change != null || itemCut != null || pickup != null || aluminum != null || domoRecarga != null)
			{

				if (itemCut != null)
				{
					texture = crosshairLaser;
				}

				else if (deposit != null && Interactable.bottleFull )
				{ 
					texture = crosshairBact;
				}

				else if (aluminum != null )
				{
					if (aluminum.HasMaterial())
					{
						texture = crosshairE;
					}
				}

				else if ( bio != null && !Biomassa.BioAlien && bio.HasAll())
				{
					texture = crosshairE;
				}

				else if (converter != null && ChangePlayer.playerBool)
				{
					texture = crosshairE;
				}

				else if (collect != null || change != null || craft != null || domoRecarga != null)
				{
			    	texture = crosshairE;
				}

				else if (bact != null && !Interactable.bottleFull )
				{
					texture = crosshairBact;
				}

				else if (pickup != null )
				{
					texture = crosshairHand;
				}
				
				
				
				
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (collect != null)
						collect.Collect();

					if (deposit != null && Ferramentas.ferramenta == "pote")
						deposit.Deposit();

					if (bact != null && Ferramentas.ferramenta == "pote")
						bact.Interact();

					if (craft != null )
						craft.OnCraftButtonClick();

					if (bio != null && !Biomassa.BioAlien)
					{
					    bio.GenerateEnergy();
	
					}

					

					if (converter != null && ChangePlayer.playerBool)
						converter.activate();

					if (change != null)
						change.ChangePlayerInteract();

					if (pickup != null && Ferramentas.ferramenta == "mão")
						pickup.Interact();

					if (aluminum != null)
					{
						if (aluminum.HasMaterial())
						{
							aluminum.ProduzirAluminum();
						}
					}

					if(domoRecarga != null && Time.timeScale != 0) 
						domoRecarga.AtivarUI();

					texture = crosshair;

				}

				if (bio != null && Biomassa.BioAlien)
				{
					CansadaDessesEfeitos.BioNoGerador = true;
				}

			}
			
		}

		else
		{
			texture = crosshair;
		}



	}

	private void OnGUI()
	{
		if (Time.timeScale == 0)
		{
			texture = null;
		}

		GUI.DrawTexture(new Rect((Screen.width / 2) - (100 / 2), (Screen.height / 2) - (100 / 2), 100, 100), texture);
	}
}
