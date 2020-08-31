using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour {

	public static float StartFuel;//start fuel
	public float maxFuel = 100f;//max fuel
	public float fuelConsumptionRate; //fuel drop rate
	public Slider fuelIndicatorSld; //slider to indicate the fuel level
	public Text fuelIndicatorTxt; //text to indicate the fuel level

	// Use this for initialization
	void Start ()
	{
		StartFuel = 100;
		///cap the fuel
		if(StartFuel > maxFuel)
		{
			StartFuel = maxFuel;
		}
		//update ui elements
		fuelIndicatorSld.maxValue = maxFuel;
		UpdateUI();
	}
	

	public void ReduceFuel()
	{
		//reduce fuel level and update ui elements
		StartFuel -= Time.deltaTime * fuelConsumptionRate;
		UpdateUI();
	}

	public void Update()
	{
		if (StartFuel >= 0)
		{
			ReduceFuel();
		}
	}


	//PICK UP JerryCan 
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("GasCan"))
		{
			StartFuel += 30;
			///cap the fuel
			if(StartFuel > maxFuel)
			{
				StartFuel = maxFuel;
			}
			UpdateUI();


			Destroy(other.gameObject);
		}
	}

	//ENTER the gas station
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("GasStation"))
		{
			StartFuel += Time.deltaTime * 5f;

			if(StartFuel > maxFuel)
			{
				StartFuel = maxFuel;
			}
			UpdateUI();
		}
	}

	void UpdateUI()
	{
		fuelIndicatorSld.value = StartFuel;
		fuelIndicatorTxt.text = "Fuel left: " + StartFuel.ToString("0") + "%";

		//if there is no fuel inform the user
		if(StartFuel <=0)
		{
			StartFuel = 0;
			fuelIndicatorTxt.text = "Out of fuel!!!";
		}
	}

	public float GetStartFuel()
	{
		return StartFuel;
	}
}
