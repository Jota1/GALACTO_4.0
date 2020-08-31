
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private float timer;
    public Text timeText;
    public GameObject[] outrasInterfaces;
    public string carregarCena;

    // Start is called before the first frame update
    void Start()
    {
        timer = 20;

        for (int i = 0; i < outrasInterfaces.Length; i++)
        {
            outrasInterfaces[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        timeText.text = "" + (int)timer;

        if (timer <= 0)
        {
            SceneManager.LoadScene(carregarCena);
        }

    }
}
