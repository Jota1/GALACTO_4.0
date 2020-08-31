using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{

    public GameObject laserPrefab;
    public Transform pontoFogo;
    private GameObject spawnedLaser;

    void Start()
    {
        spawnedLaser = Instantiate(laserPrefab, pontoFogo.position, Quaternion.identity) as GameObject;
        DisableLaser();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EnableLaser();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateLaser();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DisableLaser();
        }

    }

    void EnableLaser()
    {
        spawnedLaser.SetActive(true);
    }

    void UpdateLaser()
    {
        if (pontoFogo != null)
        {
            spawnedLaser.transform.position = pontoFogo.position;
        }
    }

    void DisableLaser()
    {
        spawnedLaser.SetActive(false);
    }
}