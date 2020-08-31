using UnityEngine;
using System.Collections;

namespace CompassNavigatorPro
{
	public class BeaconAnimator : MonoBehaviour
	{

		public float intensity = 5f;
		public float duration;

		float startingTime;
		Material mat;
		Color fullyTransparentColor, originalColor;

		// Use this for initialization
		void Awake ()
		{
			mat = GetComponent<Renderer>().material;
			fullyTransparentColor = new Color(0,0,0,0);
			originalColor = mat.color * intensity;

			startingTime = Time.time;
			duration = 1f;
			UpdateColor ();
		}

		void OnDisable() {
			DestroyBeacon();
		}
	
		// Update is called once per frame
		void Update ()
		{
			mat.mainTextureOffset =  Misc.Vector3one * Time.time * -0.25f;
			UpdateColor();
		}

		void UpdateColor() {
			float elapsed = duration<=0 ? 1f: Mathf.Clamp01( (Time.time - startingTime) / duration );
			if (elapsed>=1f) {
				DestroyBeacon();
				return;
			}
			float t =  Ease(elapsed);
			mat.color = Color.Lerp(fullyTransparentColor, originalColor, t);

		}

		float Ease(float t) {
			return Mathf.Sin (t * Mathf.PI);
		}


		void DestroyBeacon() {
			if (mat!=null) {
				DestroyImmediate(mat);
				mat = null;
			}
			if (Application.isPlaying) Destroy(gameObject);
		}
	}

}