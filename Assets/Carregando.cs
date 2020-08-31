using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Carregando : MonoBehaviour
{
    private bool carregando;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!carregando)
        {
            carregando = true;
            SceneManager.LoadSceneAsync("Level_Design_V1");
        }
    }
}
