using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovoSistemaDeConstrução : MonoBehaviour
{

    public Camera cam;
    public LayerMask layer;

    private GameObject previewGameObject = null;
    private Preview previewScript = null;

    public Button ButtonConstruir;

    [HideInInspector]
    public bool IsBuilding;
    private bool pauseBuilding = false;

    void Start()
    {
        IsBuilding = false;
    }


    // Update is called once per frame
    void Update()
    {


        if (IsBuilding)
        {
            DoBuildRay();


            if (Input.GetKeyDown(KeyCode.R))
            {
                previewGameObject.transform.Rotate(0, 90f, 0);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                CancelBuild();
                CraftingGenerator.GeneratorQuant += 1;
                ButtonConstruir.interactable = true;
            }

        }


        if (Input.GetMouseButtonDown(0) && IsBuilding)
        {
            if (previewScript.PodeConstruir())
            {
                StopBuild();
            }
            else
            {
                Debug.Log("Não Pode Construir Aqui");
            }
        }
    }

    public void NewBuild(GameObject _go)
    {
        previewGameObject = Instantiate(_go, new Vector3((Screen.width / 2) - (100 / 2), (Screen.height / 2) - (100 / 2), 0), Quaternion.identity);
        previewScript = previewGameObject.GetComponent<Preview>();
        IsBuilding = true;
    }

    private void CancelBuild()
    {
        Destroy(previewGameObject);
        previewGameObject = null;
        previewScript = null;
        IsBuilding = false;
    }

    private void StopBuild()
    {
        previewScript.Place();
        previewGameObject = null;
        previewScript = null;
        IsBuilding = false;
    }

    private void DoBuildRay()
    {
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, 100f, layer))
        {
            float y = hit.point.y + (previewGameObject.transform.localScale.y / 2f);
            Vector3 pos = new Vector3(hit.point.x, y, hit.point.z);
            previewGameObject.transform.position = pos;
            Debug.DrawRay(rayOrigin, cam.transform.forward * 100f, Color.green);

        }
    }
}
   

