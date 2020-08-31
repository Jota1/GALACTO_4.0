using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class CameraDomo : MonoBehaviour
{
    private float timer;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        timer = 7;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360 * Time.deltaTime * speed, 0);
        timer -= Time.deltaTime;

        if (timer <= 0)
            Destroy(gameObject);
    }
}
