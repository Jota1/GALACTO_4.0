using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionUI : MonoBehaviour
{
    public GameObject constructionUI;
	public GameObject[] otherUIElements;
	public GameObject BioUI;

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.C) && constructionUI.activeSelf == false && Time.timeScale != 0 && ChangePlayer.playerBool)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			UpdateInterface.CursorLock = false;

			Time.timeScale = 0;

			BioUI.SetActive(false);
			constructionUI.SetActive(true);


			for (int i = 0; i < otherUIElements.Length; i++)
			{
				otherUIElements[i].gameObject.SetActive(false);
			}
		}

		else if (Input.GetKeyDown(KeyCode.C) && constructionUI.activeSelf == true && ChangePlayer.playerBool)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			UpdateInterface.CursorLock = true;

			Time.timeScale = 1;

			constructionUI.SetActive(false);

			for (int i = 0; i < otherUIElements.Length; i++)
			{
				otherUIElements[i].gameObject.SetActive(true);
			}
		}


	}
				

	public void ActivateBio()
	{
		BioUI.SetActive(true);
	}

}
