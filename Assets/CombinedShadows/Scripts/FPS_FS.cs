using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FPS_FS : MonoBehaviour {

    Text TextC;
	// Use this for initialization
	void Start ()
    {
        TextC = this.gameObject.GetComponent<Text>();
        Application.targetFrameRate = 200;
	}
    int Frames = 0;
    float TimePassed = 0;
	// Update is called once per frame
	void Update () {
        TimePassed += Time.deltaTime;
        if (TimePassed > 1.0f)
        {
            //ParticleSystem[] ps = Object.FindObjectsOfType<ParticleSystem>();

            TextC.text = "FPS " + Frames + " " + Screen.width + " x " + Screen.height + " " + SystemInfo.graphicsDeviceType + "\n"
                + SwitchFeatures.GetCurrentFeatureString() + "\n(Press space to toggle)";

            TimePassed = 0;
            Frames = 0;            
        }
        Frames++;	
	}
}
