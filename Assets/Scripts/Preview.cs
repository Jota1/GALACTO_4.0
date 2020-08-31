using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{

    public GameObject prefab;

    private MeshRenderer myRend;
    public Material goodMat;
    public Material badMat;

    private NovoSistemaDeConstrução buildSystem;

    private bool podeConstruir = false;

    private bool inTheGround = true; 
    private bool colWithObjects = false;


    private void Start()
    {
        buildSystem = GameObject.FindObjectOfType<NovoSistemaDeConstrução>();
        myRend = GetComponent<MeshRenderer>();
        ChangeColor();
    }

    public void Place()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void ChangeColor()
    {
        if (podeConstruir)
        {
            myRend.material = goodMat;
        }
        else
        {
            myRend.material = badMat;
        }
    }

    private void Update()
    {
        if(!colWithObjects && inTheGround)
        {
            podeConstruir = true;
            ChangeColor();
        }

        else  if (colWithObjects || !inTheGround)
        {
            podeConstruir = false;
            ChangeColor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (other.CompareTag("Ground"))
            {
              
                inTheGround = true;
            }

        }

        if(!other.CompareTag("Ground"))
        {
            colWithObjects = true;
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (other.CompareTag("Ground"))
            {
               
                inTheGround = false;
            }
        }

            if (!other.CompareTag("Ground"))
            {
     
                colWithObjects = false;
            }
    }      


    public bool PodeConstruir()
    {
        return podeConstruir;
    }




}
